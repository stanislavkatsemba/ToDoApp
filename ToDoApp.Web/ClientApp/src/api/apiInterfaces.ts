export interface Result {
    isSuccessful: boolean,
    reason: string,
}

export interface UserInfo {
    userName: string,
}

export interface ToDoItemCreateInfo {
    name: string,
    description?: string,
    scheduledDate?: Date,
}

export interface ToDoItem {
    id: string,
    name: string,
    description: string,
    scheduledDate: Date,
    isCompleted: boolean,
    completionData?: Date,
    creationDate: Date,
    modificationDate?: Date,
}