import { Result } from "./apiInterfaces";

const prefix = "api";

const apiClient = {
    authentication,
    register,
    isAuthenticated,
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

function isAuthenticated(): Promise<Result> {
    let url = `${prefix}/isAuthenticated`;
    return get(url);
}

function post(url: string, data: any): Promise<Result> {
    let init: RequestInit = {
        method: "POST",
        body: JSON.stringify(data),
        headers: { "Content-Type": "application/json" }
    };
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
        return { IsSuccessful: false, Reason: error.message } as Result;
    });
}