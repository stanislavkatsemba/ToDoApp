import defaultUser from '../utils/default-user';

export async function getUser() {
    try {
        // Send request

        return {
            isOk: true,
            data: defaultUser
        };
    }
    catch {
        return {
            isOk: false
        };
    }
}
