import axios from 'axios';

export default axios.create({
    baseURL: 'http://localhost:5227',
    headers: {
        common: {
            'Authorization': 'Bearer AUTH_TOKEN'
        }
    }
});

