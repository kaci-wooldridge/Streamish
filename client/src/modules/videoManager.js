import { getToken } from "./authManager";

const baseUrl = '/api/video';

export const getAllVideos = () => {
    return getToken().then((token) => {
        return fetch(baseUrl + `/GetWithComments`, {
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

export const searchVideos = (q) => {
    return getToken().then((token) => {
        return fetch(baseUrl + `/search?q=${q}&sortDesc=true`, {
            method: "GET",
            headers: {
                Authorization: `Bearer ${token}`,
            },
        })
            .then((res) => res.json())
    })
}

export const addVideo = (video) => {
    return getToken().then((token) => {
        return fetch(baseUrl, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`,
            },
            body: JSON.stringify(video),
        });
    })
};

export const getVideo = (id) => {
    return getToken().then((token) => {
        return fetch(`${baseUrl}/${id}`, {
            method: "GET",
            headers: {
                Authorization: `Bearer ${token}`,
            },
        }).then((res) => res.json());
    })
};