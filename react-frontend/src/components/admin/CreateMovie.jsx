import React, { useState } from 'react'
import FormInput from '../FormInput'
import axios from 'axios';
import MultiSelectionInput from '../MultiSelectionInput';

function CreateMovie({ onSubmit, genres, directors }) {
    const [formData, setFormData] = useState({
        title: '',
        description: '',
        releaseDate: '',
        length: 0,
        genreIds: [],
        directorId: 0,
        posterImage: '',
        encodedImage: ''
    })

    const [error, setError] = useState('');
    const [userId, setMovieId] = useState(null);
    const [loading, setLoading] = useState(false);

    // TODO: Add separate handleChange for the selections
    const handleChange = (e) => {
        setFormData((prev) => ({
            ...prev,
            [e.target.name]: e.target.type === 'number' ? Number(e.target.value) : e.target.value,
        }));
    };

    const handleGenresChange = (e) => {
        setFormData((prev) => ({
            ...prev,
            ["genreIds"]: e,
        }));
    }

    const handleDirectorChange = (e) => {
        setFormData((prev) => ({
            ...prev,
            ["directorId"]: e,
        }));
    }

    const handleImageChange = (e) => {

        const file = e.target.files[0];

        if (file) {
            const reader = new FileReader();
            reader.onloadend = () => {
                const base64String = reader.result.replace("data:", "").replace(/^.+,/, "");

                setFormData((prev) => ({
                    ...prev,
                    ["encodedImage"]: base64String,
                    ["posterImage"]: e.target.value
                }));
            };
            reader.readAsDataURL(file);
        }
    }

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        setMovieId(null);
        setLoading(true);

        var copy = { ...formData }
        // onSubmit(copy);

        try {
            copy.genreIds = formData.genreIds.map(el => Number(el.value))
            copy.directorId = formData.directorId.value;

            // Length is set in minutes but kept in seconds
            copy.length = copy.length * 60;
            copy.posterImage = copy.encodedImage
            delete copy.encodedImage;

            console.log(copy);

            const response = await axios.post(
                'http://localhost:8080/api/media/movies/',
                copy,
                {
                    withCredentials: true,
                    headers: {
                        'Content-Type': 'application/json',
                    },
                }
            );
            setMovieId(response.data.id);
        }
        catch (err) {
            if (err.response?.status === 400) {
                console.log(err.response.data)
                setError(err.response.data);
            } else {
                setError('An unexpected error occurred.');
            }
        }

        setLoading(false);
    };

    return (
        <div className="w-[25vw] mx-auto mt-10 p-6 bg-white rounded-2xl shadow-lg">
            <h2 className="text-2xl font-bold mb-6 text-center">Create a movie</h2>
            <form onSubmit={handleSubmit} className="space-y-4">
                <FormInput label="Title" name="title" value={formData.title} onChange={handleChange} required={true} />
                <FormInput label="Description" name="description" value={formData.description} onChange={handleChange} required />
                <MultiSelectionInput label="Director" value={formData.directorId} onChange={handleDirectorChange} options={directors.map(option => ({
                    label: option.fullName,
                    value: option.id,
                }))} isMultiSelect={false} />
                <FormInput label="Length (in minutes)" name="length" value={formData.length} type="number" onChange={handleChange} required={true} />
                <MultiSelectionInput label="Genres" value={formData.genreIds} onChange={handleGenresChange} options={genres.map(option => ({
                    label: option.name,
                    value: option.id,
                }))} isMultiSelect={true} />
                <FormInput label="Picture" name="posterImage" value={formData.posterImage} type="file" onChange={handleImageChange} required />
                <FormInput label="Release Date" name="releaseDate" value={formData.releaseDate} type="date" onChange={handleChange} required={true} />


                {error && <p className="text-red-500">{JSON.stringify(error)}</p>}
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
