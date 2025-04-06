import React from 'react'
import Select from 'react-select'

function MultiSelectionInput({ label, value, onChange, isMultiSelect, options = []}) {

    const mappedOptions = options.map(option => ({
        label: option.name,
        value: option.id,
    }));

    return (
        <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">{label}</label>
            <Select
                onChange={onChange}
                options={mappedOptions}

                value={value}
                isMulti={isMultiSelect}
                className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
            >
            </Select>
        </div>)
}

export default MultiSelectionInput