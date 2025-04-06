import React, { useEffect, useState } from 'react';
import { Link, useLocation } from 'react-router-dom';
import axios from 'axios';

function NavigationBar() {

    const [isLoggedIn, setIsLoggedIn] = useState(false);
    const [isAdmin, setIsAdmin] = useState(false);
    const location = useLocation();

    useEffect(() => {
        function CheckLogin() {
            const authCookie = document.cookie.split(';').some((cookie) => cookie.trim().startsWith('IMDB_Cookie='));
            setIsLoggedIn(authCookie);
        }

        CheckLogin();
    }, [location]); // base the refresh on the location of the current page since component is shared everywhere

    useEffect(() => {
        async function CheckAdmin(){
            try {
                await axios.get(
                    'http://localhost:8080/oauth/admin',
                    {
                        withCredentials: true
                    }
                );
                setIsAdmin(true)
            }
            catch (err)
            {
                if (err.response?.status === 500)
                {
                    console.log('An unexpected error occurred.');
                }
            }
        }

        if (isLoggedIn === true)
            CheckAdmin();
    }, [isLoggedIn])

    return (
        <div className="border flex space-x-20 pl-20 py-2">

            {/* TODO: decide if I will even have a logo here. Best place would be at the start (left)*/}
            <Link to="/" className="font-bold text-blue-600">Home</Link>
            <Link to="/movies" className="font-bold text-blue-600">Movies</Link>
            <Link to="/shows" className="font-bold text-blue-600">TV Shows</Link>

            {isLoggedIn && (
                <h3 className="font-bold text-blue-600">Watchlist</h3>
            )}


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