import React, { useEffect, useState } from 'react'
import axios from 'axios';
import CreateMovie from './CreateMovie';
import CreateDirector from './CreateDirector';
import CreateActor from './CreateActor';
import CreateGenre from './CreateGenre';

function AdminPanel() {


    // Re-fetch the specific entity whenever a new one is created
    const [directorRefreshKey, setDirectorRefreshKey] = useState(0);
    const [actorRefreshKey, setActorRefreshKey] = useState(0);
    const [genreRefreshKey, setGenreRefreshKey] = useState(0);

    const [allGenres, setAllGenres] = useState([]);
    useEffect(() => {

        async function fetchGenres() {
            try {
                const res = await axios.get("http://localhost:8080/api/genres");
                console.log(res.data);
                setAllGenres(res.data);

            }
            catch (err) {
                console.error("Failed to fetch genres:", err)
            }
        }

        fetchGenres()

    }, [genreRefreshKey])

    const [directors, setDirectors] = useState([]);
    useEffect(() => {

        async function fetchDirectors() {
            try {
                const res = await axios.get("http://localhost:8080/api/directors");
                // console.log(res.data);
                setDirectors(res.data);

            }
            catch (err) {
                console.error("Failed to fetch directors:", err)
            }
        }

        fetchDirectors()

    }, [directorRefreshKey])

    
    const [actors, setActors] = useState([]);
    useEffect(() => {

        async function fetchActors() {
            try {
                const res = await axios.get("http://localhost:8080/api/actors");
                // console.log(res.data);
                setActors(res.data);

            }
            catch (err) {
                console.error("Failed to fetch actors:", err)
            }
        }

        fetchActors()

    }, [actorRefreshKey])

    // CHANGE THIS RESET FETCHING INFO ON SUCCESS
    const handleSubmit = async (formData) => {
        console.log(formData);
    }

    const handleNewDirector = () => {
        setDirectorRefreshKey(p => p + 1);
    };

    const handleNewActor = () => {
        setActorRefreshKey(p => p + 1);
    };

    const handleNewGenre = () => {
        setGenreRefreshKey(p => p + 1);
    };

    return (

        <div className="grid grid-cols-3">
            <CreateGenre onSubmit={handleNewGenre}></CreateGenre>
            
            <CreateDirector genres={allGenres} onSubmit={handleNewDirector} ></CreateDirector>
            
            <CreateActor genres={allGenres} onSubmit={handleNewActor}></CreateActor>

            <CreateMovie genres={allGenres} onSubmit={handleSubmit} directors={directors} actors={actors}></CreateMovie>
        </div>
    )
}

export default AdminPanel