import { createContext, useContext, useEffect, useState } from 'react';

const AuthContext = createContext();

const checkAuth = () => {
    return document.cookie.split(';').some((cookie) => cookie.trim().startsWith('IMDB_Cookie='));
};

export const AuthProvider = ({ children }) => {
    const [isAuthenticated, setIsAuthenticated] = useState(checkAuth());

    useEffect(() => {
        setIsAuthenticated(checkAuth());
    }, []);

    return (
        <AuthContext.Provider value={{ isAuthenticated, setIsAuthenticated }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => useContext(AuthContext);
