import React, { useState } from 'react'
import FormInput from '../FormInput'
import axios from 'axios';

function CreateMovie({ onSubmit, genres, directors }) {
    const [formData, setFormData] = useState({
        title: '',
        description: '',
        releaseDate: '',
        length: 0,
        genreIds: [],
        directorId: 0,
        posterImage: null,
    })

    const [error, setError] = useState('');
    const [userId, setMovieId] = useState(null);
    const [loading, setLoading] = useState(false);

    const handleChange = (e) => {

        setFormData((prev) => ({
            ...prev,
            [e.target.name]: e.target.type === 'number' ? Number(e.target.value) : e.target.value,
        }));
    };


    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        setMovieId(null);
        setLoading(true);

        onSubmit(formData);
        try {
            const response = await axios.post(
                'http://localhost:8080/media/movies',
                formData,
                {
                    withCredentials: true,
                    headers: {
                        'Content-Type': 'application/json',
                    },
                }
            );

            setMovieId(response.data.id);
        }
        catch (err)
        {
            if (err.response?.status === 401)
            {
                setError(err.response.data);
            } else
            {
                setError('An unexpected error occurred.');
            }
        }
        
        setLoading(false);
    };

    return (
        <div className="max-w-md mx-auto mt-10 p-6 bg-white rounded-2xl shadow-lg">
            <h2 className="text-2xl font-bold mb-6 text-center">Create a movie</h2>
            <form onSubmit={handleSubmit} className="space-y-4">
                <FormInput label="Title" name="title" value={formData.title} onChange={handleChange} required />
                <FormInput label="Description" name="description" value={formData.description} onChange={handleChange} required />
                <FormInput label="Director ID" name="directorId" value={formData.directorId} type="number" onChange={handleChange} required />
                <FormInput label="Length (in minutes))" name="length" value={formData.length} type="number" onChange={handleChange} required />

                {error && <p className="text-red-500">{error}</p>}
                {userId && <p className="text-green-600">Created movie successfully!</p>}

                <button
                    type="submit"
                    className="w-full bg-blue-600 text-white py-2 rounded-xl hover:bg-blue-700 transition"
                    disabled={loading}
                >
                    {loading ? 'Creating movie...' : 'Create movie'}
                </button>
            </form>
        </div>
    )
}

export default CreateMovie
