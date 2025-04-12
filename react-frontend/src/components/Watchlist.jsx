import React, { useEffect, useState } from 'react'
import axios from 'axios'
import WatchlistDisplayTable from './WatchlistDisplayTable';

function Watchlist() {

    const [mediaList, setMediaList] = useState([])
    const [watchlistRefreshKey, setWatchlistRefreshKey] = useState(0);

    const handleWatchlistRefresh = () => {
        setWatchlistRefreshKey(p => p + 1);
    };

    useEffect(() => {
        async function fetchMedia() {
            try {
                const res = await axios.get("http://localhost:8080/api/watchlist/details", {
                    withCredentials: true,
                    headers: {
                        'Content-Type': 'application/json',
                    },
                });
                // console.log(res.data);
                setMediaList(res.data.media);
            }
            catch (err) {
                console.error("Failed to fetch media:", err)
            }
        }

        fetchMedia()
    }, [watchlistRefreshKey])

    return (
        <>
            <WatchlistDisplayTable mediaList={mediaList} handleRefresh={handleWatchlistRefresh}></WatchlistDisplayTable>
        </>
    )
}

export default Watchlist