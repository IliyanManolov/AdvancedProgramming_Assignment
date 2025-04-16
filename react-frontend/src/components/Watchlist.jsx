import React, { useEffect, useState } from 'react'
import axios from 'axios'
import WatchlistDisplayTable from './WatchlistDisplayTable';
import { useWatchlist } from './contexts/WatchlistContext';

function Watchlist() {

    const { watchlistDict: mediaDict, watchlist: mediaList, TryDeleteElement } = useWatchlist();

    return (
        <>
            <WatchlistDisplayTable mediaList={mediaList} handleRefresh={TryDeleteElement} watchlistDict={mediaDict}></WatchlistDisplayTable>
        </>
    )
}

export default Watchlist