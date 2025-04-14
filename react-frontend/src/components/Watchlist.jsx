import React, { useEffect, useState } from 'react'
import axios from 'axios'
import WatchlistDisplayTable from './WatchlistDisplayTable';

function Watchlist() {

    const [mediaList, setMediaList] = useState([]);
    const [mediaDict, setMediaDict] = useState({});
    const [watchlistRefreshKey, setWatchlistRefreshKey] = useState(0);

    const handleWatchlistRefresh = async (type, id) => {
        if (mediaDict[type][id]) {
            try {
                // Manually assemble request because axios delete does not allow sending bodies?
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
                setWatchlistRefreshKey(p => p + 1);
            }
            catch (err) {
                console.error("Failed to fetch media:", err)
            }
        }
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