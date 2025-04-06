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

    const handleSubmit = async (formData) => {
        formData.length = formData.length * 60;
        console.log(formData);
    }

    return (

        <>
            {/* TODO: decide how to handle genres & directors */}
            <CreateMovie genres={allGenres} onSubmit={handleSubmit} directors={[]}></CreateMovie>
        </>
    )
}

export default AdminPanel