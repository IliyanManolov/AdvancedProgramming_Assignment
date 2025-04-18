import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import axios from 'axios';
import { useAuth } from './contexts/AuthContext';
import { useWatchlist } from './contexts/WatchlistContext';

function NavigationBar() {

    const { isAuthenticated: isLoggedIn } = useAuth();
    const [isAdmin, setIsAdmin] = useState(false);
    const { watchlistCount } = useWatchlist();

    useEffect(() => {
        async function CheckAdmin() {
            try {
                await axios.get(
                    'http://localhost:8080/oauth/admin',
                    {
                        withCredentials: true
                    }
                );
                setIsAdmin(true)
            }
            catch (err) {
                if (err.response?.status === 500) {
                    console.log('An unexpected error occurred.');
                }
            }
        }

        if (isLoggedIn === true)
            CheckAdmin();

    }, [isLoggedIn])

    return (
        <div className="border flex justify-between px-20 pl-20 py-2">
            <div className="flex items-center space-x-20">
                <Link to="/" className="font-bold text-blue-600">Home</Link>
                <Link to="/movies" className="font-bold text-blue-600">Movies</Link>
                <Link to="/shows" className="font-bold text-blue-600">TV Shows</Link>
            {isLoggedIn && (
                <Link to="/watchlist" className="font-bold text-blue-600">Watchlist ({watchlistCount} remaining)</Link>
            )}
            </div>

            {/* TODO: decide if I will even have a logo here*/}

            <div className="flex items-center space-x-10">
                {/* Conditional rendering of login/register page*/}
                {!isLoggedIn && (
                    <>
                        <Link to="/login" className="font-bold text-blue-600">Login</Link>
                        <Link to="/register" className="font-bold text-blue-600">Register</Link>
                    </>
                )}

                {isAdmin && (
                    <>
                        <Link to="/adminpanel" className="font-bold text-blue-600">Administrator Panel</Link>
                    </>
                )}
            </div>
        </div>
        
    )
}

export default NavigationBar