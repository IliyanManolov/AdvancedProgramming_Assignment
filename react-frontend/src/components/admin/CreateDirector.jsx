import React, { useState } from 'react'
import FormInput from '../inputs/FormInput'
import axios from 'axios';

function CreateDirector({ onSubmit }) {

    const [formData, setFormData] = useState({
        firstName: '',
        lastName: '',
        biography: '',
        birthDate: '',
        dateOfDeath: '',
        nationality: '',
        profileImage: '',
        encodedImage: ''
    })

    const [error, setError] = useState('');
    const [userId, setDirectorId] = useState(null);
    const [loading, setLoading] = useState(false);

    const handleChange = (e) => {
        setFormData((prev) => ({
            ...prev,
            [e.target.name]: e.target.type === 'number' ? Number(e.target.value) : e.target.value,
        }));
    };

    const handleImageChange = (e) => {

        const file = e.target.files[0];

        if (file) {
            const reader = new FileReader();
            reader.onloadend = () => {
                const base64String = reader.result.replace("data:", "").replace(/^.+,/, "");

                setFormData((prev) => ({
                    ...prev,
                    ["encodedImage"]: base64String,
                    ["profileImage"]: e.target.value
                }));
            };
            reader.readAsDataURL(file);
        }
    }


    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        setDirectorId(null);
        setLoading(true);

        var copy = { ...formData }

        try {
            copy.profileImage = copy.encodedImage
            delete copy.profileImage;

            if (copy.dateOfDeath === '')
                copy.dateOfDeath = null;

            // console.log(copy);

            const response = await axios.post(
                'http://localhost:8080/api/directors',
                copy,
                {
                    withCredentials: true,
                    headers: {
                        'Content-Type': 'application/json',
                    },
                }
            );
            setDirectorId(response.data.id);
            onSubmit();
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
            <h2 className="text-2xl font-bold mb-6 text-center">Create a director</h2>
            <form onSubmit={handleSubmit} className="space-y-4">

                <FormInput label="First Name" name="firstName" value={formData.firstName} onChange={handleChange} required={true} />
                <FormInput label="Last Name" name="lastName" value={formData.lastName} onChange={handleChange} required={true} />
                <FormInput label="Biography" name="biography" value={formData.biography} onChange={handleChange} required />
                <FormInput label="Nationality" name="nationality" value={formData.nationality} onChange={handleChange} required={true} />
                <FormInput label="Picture" name="profileImage" value={formData.profileImage} type="file" onChange={handleImageChange} required />
                <FormInput label="Birth Date" name="birthDate" value={formData.birthDate} type="date" onChange={handleChange} required={true} />
                <FormInput label="Date of death" name="dateOfDeath" value={formData.dateOfDeath} type="date" onChange={handleChange} required={false} />

                {error && <p className="text-red-5S00">{JSON.stringify(error)}</p>}
                {userId && <p className="text-green-600">Created director successfully!</p>}

                <button
                    type="submit"
                    className="w-full bg-blue-600 text-white py-2 rounded-xl hover:bg-blue-700 transition"
                    disabled={loading}
                >
                    {loading ? 'Creating director...' : 'Create director'}
                </button>
            </form>
        </div>
    )
}

export default CreateDirector