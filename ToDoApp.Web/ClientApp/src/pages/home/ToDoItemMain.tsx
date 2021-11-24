import React from 'react';
import { ToDoItem } from '../../api/apiInterfaces';
import CheckBox from 'devextreme-react/check-box';

type ToDoItemMainProps = {
    item: ToDoItem,
    onComplete: (id: string) => void,
    onRevokeCompletion: (id: string) => void,
}

export const ToDoItemMain = ({ item, onComplete, onRevokeCompletion }: ToDoItemMainProps) => {

    const onCompleteClick = React.useCallback((value: any) => {
        if (value === true && onComplete) {
            onComplete(item.id);
        }
        if (value === false && onRevokeCompletion) {
            onRevokeCompletion(item.id);
        }
    }, []);

    return (
        <>
            <CheckBox
                value={item.isCompleted}
                onValueChange={onCompleteClick}
            />&nbsp;&nbsp;&nbsp;
            <span
                className={item.isCompleted ? "item-name-completed" : ""}
            >
                {item.isCompleted ? <> {new Date(item.completionData).toLocaleDateString()}&nbsp;</> : ""}{item.name}
            </span>
            <div className="item-description">
                {item.description}
            </div>
        </>
    );
}
