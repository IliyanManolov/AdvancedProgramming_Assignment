import React, { useEffect, useState } from 'react'
import axios from 'axios'
import WatchlistDisplayTable from './WatchlistDisplayTable';

function Watchlist() {

    const [mediaList, setMediaList] = useState([])

    useEffect(() => {
        async function fetchMedia() {
            try {
                const res = await axios.get("http://localhost:8080/api/watchlist/details", {
                    withCredentials: true,
                    headers: {
                        'Content-Type': 'application/json',
                    },
                });
                console.log(res.data);
                setMediaList(res.data.media);
            }
            catch (err) {
                console.error("Failed to fetch media:", err)
            }
        }

        fetchMedia()
    }, [])

    return (
        <>
        <WatchlistDisplayTable mediaList={mediaList}></WatchlistDisplayTable>
        </>
    )
}

export default Watchlist