import React from 'react';
import { ToDoItem } from '../../api/apiInterfaces';
import CheckBox from 'devextreme-react/check-box';
import DateBox from 'devextreme-react/date-box';
import Box, { Item } from 'devextreme-react/box';

interface ToDoItemMainProps {
    item: ToDoItem,
    onComplete: (id: string) => void,
    onRevokeCompletion: (id: string) => void,
    onSchedule: (id: string, date: Date) => void,
    onClearScheduling: (id: string) => void,
}

export const ToDoItemMain = ({ item, onComplete, onRevokeCompletion, onSchedule, onClearScheduling }: ToDoItemMainProps) => {

    const onCompleteClick = React.useCallback((value: any) => {
        if (value === true && onComplete) {
            onComplete(item.id);
        }
        if (value === false && onRevokeCompletion) {
            onRevokeCompletion(item.id);
        }
    }, [onComplete, onRevokeCompletion, item.id]);

    const onSchedulingChange = React.useCallback((value: any) => {
        if (value && onSchedule) {
            onSchedule(item.id, value);
        }
        if (value === null && onClearScheduling) {
            onClearScheduling(item.id);
        }
    }, [onSchedule, onClearScheduling, item.id]);

    return (
        <>
            <Box
                direction="row"
                width="100%"
                align="center"
                crossAlign="center"
            >
                <Item
                    ratio={1}
                >
                    <div>
                        <div className="todoitem-date">
                            Erstellt {new Date(item.creationDate).toLocaleDateString()}
                            {item.modificationDate ? <>, ge&auml;ndert {new Date(item.modificationDate).toLocaleDateString()} </> : ""}
                        </div>
                        <CheckBox
                            value={item.isCompleted}
                            onValueChange={onCompleteClick}
                        />&nbsp;&nbsp;&nbsp;
                        <span
                            className={item.isCompleted ? "todoitem-name-completed todoitem-name" : "todoitem-name"}
                        >
                            {item.isCompleted && item.completionData ? <> {new Date(item.completionData).toLocaleDateString()}&nbsp;</> : ""}{item.name}
                        </span>
                        <div className="todoitem-description">
                            {item.description}
                        </div>
                    </div>
                </Item>
                <Item
                    ratio={0}
                    baseSize={150}
                >
                    <div>
                        <div className="todoitem-scheduling-caption">Planen</div>
                        <DateBox
                            value={item.scheduledDate}
                            readOnly={item.isCompleted}
                            min={new Date()}
                            showClearButton={true}
                            onValueChange={onSchedulingChange}
                        />
                    </div>
                </Item>
            </Box>
        </>
    );
}
