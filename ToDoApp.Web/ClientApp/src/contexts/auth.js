import React, { useState, useEffect, createContext, useContext, useCallback } from 'react';
import apiClient from '../api/apiClient';

function AuthProvider(props) {
    const [user, setUser] = useState();
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        (async function () {
            const result = await apiClient.getUser();
            if (result && result.userName) {
                setUser({ email: result.userName });
            }
            setLoading(false);
        })();
    }, []);

    const signIn = useCallback(async (email, password) => {
        const result = await apiClient.authentication(email);
        if (result.isSuccessful) {
            setUser({ email: email });
        }

        return result;
    }, []);

    const signOut = useCallback(async () => {
        const result = await apiClient.logout();
        if (result.isSuccessful) {
            setUser();
        }
    }, []);


    return (
        <AuthContext.Provider value={{ user, signIn, signOut, loading }} {...props} />
    );
}

const AuthContext = createContext({});
const useAuth = () => useContext(AuthContext);

export { AuthProvider, useAuth }
