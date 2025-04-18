import React from 'react'

function SlideInput({ label, name, value, min, max, onChange, stepRate, type = 'number', required = false }) {
    return (
        <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">{label}</label>
            <input
                type={type}
                name={name}
                min={min}
                max={max}
                step={stepRate}
                value={value}
                onChange={onChange}
                required={required}
                className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
            />
        </div>
    )
}

export default SlideInput