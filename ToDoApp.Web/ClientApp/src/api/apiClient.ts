import { Result } from "./apiInterfaces";

const prefix = "api";

const apiClient = {
    authentication,
    register,
    getUser,
    logout,
}

export default apiClient;

function authentication(userName: string): Promise<Result> {
    let url = `${prefix}/authentication`;
    return post(url, userName);
}

function register(userName: string): Promise<Result> {
    let url = `${prefix}/register`;
    return post(url, userName);
}

function getUser(): Promise<Result> {
    let url = `${prefix}/user`;
    return get(url);
}

function logout(): Promise<Result> {
    let url = `${prefix}/logout`;
    return post(url);
}

function post(url: string, data?: any): Promise<Result> {
    let init: RequestInit = { method: "POST" };
    if (data) {
        init.body = JSON.stringify(data);
        init.headers = { "Content-Type": "application/json" };
    }
    return defaultFetch(url, init);
}

function get(url: string): Promise<Result> {
    let init: RequestInit = { method: "GET" };
    return defaultFetch(url, init);
}

function defaultFetch(url: string, init: RequestInit): Promise<Result> {
    return fetch(url, init).then(response => {
        if (response.ok) {
            return response.json().then((data: Result) => data);
        }
        throw new Error(response.statusText);
    }).catch((error: Error) => {
        return { isSuccessful: false, reason: error.message } as Result;
    });
}