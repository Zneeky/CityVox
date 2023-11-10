import axios from 'axios';
import { CLOUDINARY_UPLOAD_PRESET, CLOUDINARY_UPLOAD_URL, httpsApiCode } from './consts';

//img upload to cloudinary.com anonymous
export const uploadToCloudinary = async (file) => {
    const formData = new FormData();
    formData.append("file", file)
    formData.append("upload_preset", CLOUDINARY_UPLOAD_PRESET)
    try {
        const response = await axios.post(`${CLOUDINARY_UPLOAD_URL}`, formData)
    
        const img = await response.data;
        return img.secure_url
    } catch (error) {
        console.error("Error uploading image:", error);
    }
}

//registration api call anonymous
export const RegisterUser =  async (values, onSubmitProps) => {
    if(values.profilePicture){
        const imgUrl = await uploadToCloudinary(values.profilePicture);
        values.profilePicture = imgUrl;
    }
    try {
        const response = await axios.post(
            `${httpsApiCode}api/auth/register`, 
            values
        )
        return response;
    } catch (error) {
        console.error("Unsuccessful registration", error);
    }
}

//login with credentials api call anonymous
export const LoginUser = async (values, onSubmitProps) => {
    try {
        const response = await axios.post(
            `${httpsApiCode}api/auth/login`,
            values
        )
        
        return response;
    } catch (error) {
        console.error("Unsuccessful login", error);
    }
}

//login with refreshToken api call anonymous
export const LoginRefresh =  async (accessToken) => {
    try {
        const response = await axios.get(
            `${httpsApiCode}api/auth/login/refres?JwtToken=${accessToken}`
        )

        return response;
    }catch (error){
        console.error("Restricted! Log in with credentials", error);
    }
}

//logout to delete refreshToken api call anonymous
export const LogoutRefresh = async () => {
    try {
        const response = await axios.get(
            `${httpsApiCode}api/auth/logout`
        )
        return response;

    }catch (error){
        console.error("Log in with credentials", error);
    }
}