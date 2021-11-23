import { default as dxNotify }  from 'devextreme/ui/notify';

const notification = {
    notify,
    success,
    error,
    info,
    warning
}

export default notification;

const defaultDisplayTime = 5000; //ms

function notify(message: string, type: 'success' | 'error' | 'info' | 'warning', displayTime?: number) {
    dxNotify({ message: message, closeOnClick: false, closeOnWipe: false, closeOnOutsideClick: false }, type, displayTime ?? defaultDisplayTime);
}

function success(message: string, displayTime?: number) {
    notify(message, 'success', displayTime);
}

function error(message: string, displayTime?: number) {
    notify(message, 'error', displayTime);
}

function info(message: string, displayTime?: number) {
    notify(message, 'info', displayTime);
}

function warning(message: string, displayTime?: number) {
    notify(message, 'warning', displayTime);
}