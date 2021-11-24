import React from 'react';
import './home.scss';
import { HubConnectionBuilder, HubConnection } from '@microsoft/signalr';
import DataGrid, { Pager, Paging, Column, Editing, Form, FormItem } from 'devextreme-react/data-grid';
import CustomStore from 'devextreme/data/custom_store';
import DataSource from 'devextreme/data/data_source';
import notification from '../../utils/notification';
import { ToDoItem, ToDoItemDto } from '../../api/apiInterfaces';
import nameof from 'ts-nameof.macro';
import { CheckBox } from 'devextreme-react/check-box';
import { RequiredRule, SimpleItem } from 'devextreme-react/form';
import "devextreme/ui/text_area";
import { EventInfo } from 'devextreme/events';
import dxDataGrid, { RowUpdatingInfo } from 'devextreme/ui/data_grid';

interface IHomeState {
    connectionStarted: boolean,
    dataSource: DataSource,
}

export default class Home extends React.Component<{}, IHomeState> {

    hubConnection: HubConnection = null as any;
    store: CustomStore<ToDoItem, string> = null as any;

    constructor(props: {}) {
        super(props);
        this.state = { connectionStarted: false, dataSource: null as any };
    }

    componentDidMount() {
        this.hubConnection = new HubConnectionBuilder()
            .withUrl('/hubs/toDoItems')
            .withAutomaticReconnect()
            .build();

        this.store = new CustomStore<ToDoItem, string>({
            load: () => this.hubConnection.invoke('GetAllToDoItems'),
            insert: (newItem) => {
                const item: ToDoItemDto = {
                    name: newItem.name,
                    description: newItem.description,
                };
                this.hubConnection.send("CreateToDoItem", item).catch(_ => {
                    this.notificateNoConnectionToHub();
                });
                return Promise.resolve({} as any);
            },
            update: (itemId, newItem) => {
                const item: ToDoItemDto = {
                    id: itemId,
                    name: newItem.name,
                    description: newItem.description,
                };
                this.hubConnection.send("UpdateToDoItem", item).catch(_ => {
                    this.notificateNoConnectionToHub();
                });
                return Promise.resolve({} as any);
            },
            remove: (itemId) => {
                this.hubConnection.send("RemoveToDoItem", itemId).catch(_ => {
                    this.notificateNoConnectionToHub();
                });
                return Promise.resolve();
            },
            key: nameof<ToDoItem>(x => x.id),
        });

        const dataSource = new DataSource({ store: this.store, reshapeOnPush: true });

        this.hubConnection
            .start()
            .then(() => {
                this.hubConnection.on('ReceiveToDoItem', (data: ToDoItem) => {
                    this.store.push([{ type: "remove", key: data.id }, { type: "insert", data: data }]);
                });
                this.hubConnection.on('ToDoItemRemoved', (id: string) => {
                    this.store.push([{ type: "remove", key: id }]);
                });
                this.setState({ connectionStarted: true, dataSource: dataSource });
            });
    }

    componentWillUnmount() {
        this.hubConnection.stop();
    }

    render() {

        return (
            <>
                <h2 className={'content-block'}>Aufgaben</h2>
                <div className={'content-block'}>
                    <div className={'dx-card responsive-paddings'}>
                        {this.state.connectionStarted
                            &&
                            <DataGrid
                                className={'dx-card wide-card'}
                                dataSource={this.state.dataSource}
                                showBorders={false}
                                columnHidingEnabled={true}
                                showColumnHeaders={false}
                                onRowUpdating={this.onRowUpdating}
                            >
                                <Editing
                                    mode="form"
                                    allowUpdating={true}
                                    allowAdding={true}
                                    allowDeleting={true}
                                >
                                    <Form
                                        showValidationSummary={true}
                                        colCount={1}
                                    >
                                        <SimpleItem dataField={nameof<ToDoItem>(x => x.name)} />
                                        <SimpleItem dataField={nameof<ToDoItem>(x => x.description)} />
                                    </Form>
                                </Editing>
                                <Paging defaultPageSize={10} />
                                <Pager showPageSizeSelector={true} showInfo={true} />
                                <Column
                                    dataField={nameof<ToDoItem>(x => x.id)}
                                    cellRender={this.cellRender}
                                />
                                <Column
                                    dataField={nameof<ToDoItem>(x => x.name)}
                                    visible={false}
                                    caption="Theme"
                                >
                                    <RequiredRule message="Thema ist erforderlich" />
                                    <FormItem editorType="dxTextBox" editorOptions={nameEditorOptions} />
                                </Column>
                                <Column
                                    caption="Beschreibung"
                                    dataField={nameof<ToDoItem>(x => x.description)}
                                    visible={false}
                                >
                                    <FormItem editorType="dxTextArea" editorOptions={descriptionEditorOptions} />
                                </Column>
                                <Column
                                    caption="Planen"
                                    dataField={nameof<ToDoItem>(x => x.scheduledDate)}
                                    visible={false}
                                />
                                <Column
                                    caption="Planen"
                                    dataField={nameof<ToDoItem>(x => x.creationDate)}
                                    visible={false}
                                    sortOrder="desc"
                                />
                            </DataGrid>
                        }
                    </div>
                </div>
            </>
        );
    }

    onRowUpdating = (e: EventInfo<dxDataGrid<ToDoItem, any>> & RowUpdatingInfo<ToDoItem, any>) => {
        if (e.newData.name === undefined) {
            e.newData.name = e.oldData.name;
        }
        if (e.newData.description === undefined) {
            e.newData.description = e.oldData.description;
        }
    }

    cellRender = (e: { data: ToDoItem }) => {
        return (
            <>
                <CheckBox
                    value={e.data.isCompleted}
                    onValueChange={this.onCompletedValueChanged.bind(null, e.data)}
                />
                &nbsp;&nbsp;&nbsp;
                {e.data.name}
                <div className="item-description">{e.data.description}</div>
            </>
        );
    }

    onCompletedValueChanged = (toDoItem: ToDoItem, value: any) => {
        if (value === true) {
            this.hubConnection.send("CompleteToDoItem", toDoItem.id).catch(_ => {
                this.notificateNoConnectionToHub();
            });
        }
        if (value === false) {
            this.hubConnection.send("RevokeCompletionToDoItem", toDoItem.id).catch(_ => {
                this.notificateNoConnectionToHub();
            });
        }
    }

    notificateNoConnectionToHub() {
        notification.error('Keine Verbindung zum Server.');
    }
}

const nameEditorOptions = { maxLength: 250 }
const descriptionEditorOptions = { maxLength: 1000 }
