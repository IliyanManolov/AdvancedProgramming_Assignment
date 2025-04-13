import React, { useEffect, useState } from 'react'
import axios from 'axios'
import WatchlistDisplayTable from './WatchlistDisplayTable';

function Watchlist() {

    const [mediaList, setMediaList] = useState([]);
    const [mediaDict, setMediaDict] = useState({});
    const [watchlistRefreshKey, setWatchlistRefreshKey] = useState(0);

    const handleWatchlistRefresh = (type, id) => {
        console.log(type);
        console.log(id);

        // Send DELETE request for the specific element
        
        // If success -> refresh key
        // else - ? error in the top of the screen ?

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


                const watchListDict = res.data.media.reduce((dict, item) => {
                    if (!dict[item.type]) {
                        dict[item.type] = {};
                    }
                    dict[item.type][item.id] = item.title;
                    return dict;
                }, {});

                setMediaDict(watchListDict)
            }
            catch (err) {
                console.error("Failed to fetch media:", err)
            }
        }

        fetchMedia()
    }, [watchlistRefreshKey])

    return (
        <>
            <WatchlistDisplayTable mediaList={mediaList} handleRefresh={handleWatchlistRefresh} watchlistDict={mediaDict}></WatchlistDisplayTable>
        </>
    )
}

export default Watchlist