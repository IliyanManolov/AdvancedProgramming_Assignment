import React from 'react';

function NavigationBar() {
    return (
        <div className="border flex space-x-20 pl-20 py-2">

            {/* TODO: migrate to buttons? or just links for navigation */}
            {/* TODO: decide if I will even have a logo here. Best place would be at the start (left)*/}
            <h3 className="font-bold text-blue-600">Movies</h3>
            <h3 className="font-bold text-blue-600">TV Shows</h3>
            <h3 className="font-bold text-blue-600">Watchlist</h3>

        </div>
    )
}

export default NavigationBar