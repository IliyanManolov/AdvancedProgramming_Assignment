import React, { useState } from 'react'
import FormInput from '../FormInput'
import axios from 'axios';
import MultiSelectionInput from '../MultiSelectionInput';

function CreateShowEpisode({ shows }) {

    const [formData, setFormData] = useState({
        title: '',
        description: '',
        dateAired: '',
        length: 0,
        showId: 0,
        seasonNumber: 0
    })

    const [error, setError] = useState('');
    const [userId, setEpisodeId] = useState(null);
    const [loading, setLoading] = useState(false);

    const handleShowChange = (e) => {
        setFormData((prev) => ({
            ...prev,
            ["showId"]: e,
        }));
    }

    const handleChange = (e) => {
        console.log(shows)
        setFormData((prev) => ({
            ...prev,
            [e.target.name]: e.target.type === 'number' ? Number(e.target.value) : e.target.value,
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        setEpisodeId(null);
        setLoading(true);

        var copy = { ...formData }
        // onSubmit(copy);

        try {
            // Length is set in minutes but kept in seconds
            copy.length = copy.length * 60;
            copy.posterImage = copy.encodedImage
            delete copy.encodedImage;

            copy.showId = Number(copy.showId.value)
            // console.log(copy);

            const response = await axios.post(
                'http://localhost:8080/api/media/episodes/',
                copy,
                {
                    withCredentials: true,
                    headers: {
                        'Content-Type': 'application/json',
                    },
                }
            );
            setEpisodeId(response.data.id);
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
            <h2 className="text-2xl font-bold mb-6 text-center">Create a show episode</h2>
            <form onSubmit={handleSubmit} className="space-y-4">
                <FormInput label="Title" name="title" value={formData.title} onChange={handleChange} required={true} />
                <FormInput label="Description" name="description" value={formData.description} onChange={handleChange} required />
                <FormInput label="Length (in minutes)" name="length" value={formData.length} type="number" onChange={handleChange} required={true} />
                <MultiSelectionInput label="Show" value={formData.showId} onChange={handleShowChange} options={shows.map(option => ({
                    label: option.title,
                    value: option.id,
                }))} isMultiSelect={false} />
                <FormInput label="Season number" name="seasonNumber" value={formData.seasonNumber} type="number" onChange={handleChange} required={true} />
                <FormInput label="Date aired" name="dateAired" value={formData.dateAired} type="date" onChange={handleChange} required={true} />


                {error && <p className="text-red-500">{JSON.stringify(error)}</p>}
                {userId && <p className="text-green-600">Created show episode successfully!</p>}

                <button
                    type="submit"
                    className="w-full bg-blue-600 text-white py-2 rounded-xl hover:bg-blue-700 transition"
                    disabled={loading}
                >
                    {loading ? 'Creating show episode...' : 'Create show episode'}
                </button>
            </form>
        </div>
    )
}

export default CreateShowEpisode