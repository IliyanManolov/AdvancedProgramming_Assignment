import { createContext, useContext, useEffect, useState } from 'react';
import { useAuth } from './AuthContext';
import axios from 'axios';


const WatchlistContext = createContext();

export const WatchlistProvider = ({ children }) => {

    const { isAuthenticated } = useAuth();

    const [watchlistDict, setWatchlistDict] = useState({});
    const [watchlist, setWatchlist] = useState([]);
    const [watchlistCount, setWatchlistCount] = useState();

    const RefreshWatchlist = async () => {
        if (!isAuthenticated)
            return;

        try {
            const res = await axios.get("http://localhost:8080/api/watchlist/details", {
                withCredentials: true,
                headers: {
                    'Content-Type': 'application/json',
                },
            });
            // console.log(res.data);
            setWatchlist(res.data.media);

            const tmpDict = res.data.media.reduce((dict, item) => {
                if (!dict[item.type]) {
                    dict[item.type] = {};
                }
                dict[item.type][item.id] = item.title;
                return dict;
            }, {});

            setWatchlistDict(tmpDict)
        }
        catch (err) {
            console.error("Failed to fetch watchlist:", err)
        }

        try {
            const res = await axios.get(
                'http://localhost:8080/api/watchlist',
                {
                    withCredentials: true
                }
            );
            setWatchlistCount(res.data.mediaCount)
        }
        catch (err) {
            if (err.response?.status === 500) {
                console.log('An unexpected error occurred.');
            }
        }
    };

    const TryAddElement = async (id, type) => {
        try {
            const res = await axios.request({
                "url": "http://localhost:8080/api/watchlist",
                "method": "POST",
                "data": {
                    "Type": type,
                    "Id": Number(id)
                },
                withCredentials: true,
                headers: {
                    'Content-Type': 'application/json'
                }
            })
            RefreshWatchlist();
        }
        catch (err) {
            console.error("Failed to add to watchlist:", err)
        }
    }

    const TryDeleteElement = async (id, type) => {
        try {
            const res = await axios.request({
                "url": "http://localhost:8080/api/watchlist",
                "method": "DELETE",
                "data": {
                    "Type": type,
                    "Id": Number(id)
                },
                withCredentials: true,
                headers: {
                    'Content-Type': 'application/json'
                }
            })
            RefreshWatchlist();
        }
        catch (err) {
            console.error("Failed to remove from watchlist:", err)
        }
    }

    // Generic method to abstract logic for choosing add/delete
    const ChangeElement = async (id, type) => {
        if (watchlistDict[type] && watchlistDict[type][id])
            await TryDeleteElement(id, type);
        else
            await TryAddElement(id, type);
    }

    useEffect(() => {
        if (isAuthenticated) {
            RefreshWatchlist();
        }
    }, [isAuthenticated]);

    return (
        <WatchlistContext.Provider value={{ watchlistCount, watchlistDict, watchlist, TryDeleteElement, TryAddElement, ChangeElement }}>
            {children}
        </WatchlistContext.Provider>
    )
}

export const useWatchlist = () => useContext(WatchlistContext);