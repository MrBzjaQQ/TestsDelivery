import { STORE_KEY } from "../../infrastructure/constants/localStorageConstants";

export interface LocalStore {
    accessToken?: string;
}

class LocalStorage {
    public setAuthToken(token: string) {
        const store = JSON.stringify(window.localStorage.getItem(STORE_KEY)) as LocalStore;

        const updatedStore = {
            ...store,
            accessToken: token
        };

        window.localStorage.setItem(STORE_KEY, JSON.stringify(updatedStore));
    };

    public getAuthToken(): string | undefined {
        const store = JSON.stringify(window.localStorage.getItem(STORE_KEY)) as LocalStore;

        return store.accessToken;
    }
}

export default new LocalStorage();