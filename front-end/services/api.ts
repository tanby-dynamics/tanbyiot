import axios from 'axios';

export default axios.create({
    baseURL: 'https://localhost:7128',
    headers: {
        common: {
            'Authorization': 'Bearer AUTH_TOKEN'
        }
    }
});

