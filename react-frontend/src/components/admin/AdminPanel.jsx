import React, { useEffect, useState } from 'react'
import axios from 'axios';
import CreateMovie from './CreateMovie';

function AdminPanel() {

    const [allGenres, setAllGenres] = useState([]);

    useEffect(() => {

        async function fetchGenres() {
            try {
                const res = await axios.get("http://localhost:8080/api/genres");
                // console.log(res.data);
                setAllGenres(res.data);

            }
            catch (err) {
                console.error("Failed to fetch genres:", err)
            }
        }

        fetchGenres()

    }, [])

    const [directors, setDirectors] = useState([]);

    useEffect(() => {

        async function fetchDirectors() {
            try {
                const res = await axios.get("http://localhost:8080/api/directors");
                // console.log(res.data);
                setDirectors(res.data);

            }
            catch (err) {
                console.error("Failed to fetch genres:", err)
            }
        }

        fetchDirectors()

    }, [])

    // CHANGE THIS RESET FETCHING INFO ON SUCCESS
    const handleSubmit = async (formData) => {
        console.log(formData);
    }

    return (

        <div className="grid grid-cols-3">
            <CreateMovie genres={allGenres} onSubmit={handleSubmit} directors={directors}></CreateMovie>
            
            {/* TEMP FOR TESTING PURPOSES */}
            <CreateMovie genres={allGenres} onSubmit={handleSubmit} directors={directors}></CreateMovie>
            
            <CreateMovie genres={allGenres} onSubmit={handleSubmit} directors={directors}></CreateMovie>
        </div>
    )
}

export default AdminPanel