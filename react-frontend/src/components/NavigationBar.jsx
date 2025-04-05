import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';

function NavigationBar() {

    const [isLoggedIn, setIsLoggedIn] = useState(false);

    useEffect(() => {
        function CheckLogin() {
            const authCookie = document.cookie.split(';').some((cookie) => cookie.trim().startsWith('IMDB_Cookie='));
            setIsLoggedIn(authCookie);
        }

        CheckLogin();
    }, []);

    return (
        <div className="border flex space-x-20 pl-20 py-2">

            {/* TODO: decide if I will even have a logo here. Best place would be at the start (left)*/}
            <Link to="/" className="font-bold text-blue-600">Home</Link>
            <Link to="/movies" className="font-bold text-blue-600">Movies</Link>
            <h3 className="font-bold text-blue-600">TV Shows</h3>
            <h3 className="font-bold text-blue-600">Watchlist</h3>

            {/* Conditional rendering again */}
            {!isLoggedIn && (
                <>
                    <Link to="/login" className="font-bold text-blue-600">Login</Link>
                    <Link to="/register" className="font-bold text-blue-600">Register</Link>
                </>
            )}
        </div>
    )
}

export default NavigationBar