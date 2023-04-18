import { getToken } from "./authManager";

const baseUrl = '/api/UserProfile';

export const getAllUsers = () => {
    return getToken().then((token) => {
        return fetch(`${baseUrl}`, {
            method: "GET",
            headers: {
                Authorization: `Bearer ${token}`,
            },
        })
            .then((res) => {
                if (res.ok) {
                    return res.json();
                } else {
                    throw new Error(
                        "An unknown error occurred.",
                    );
                }
            })
    })
};

export const getUser = (id) => {
    return getToken().then((token) => {
        return fetch(`${baseUrl}/getWithVideosAndComments${id}`, {
            method: "GET",
            headers: {
                Authorization: `Bearer ${token}`,
            },
        }).then((res) => res.json());
    })
};

