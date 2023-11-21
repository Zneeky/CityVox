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

//Registration api call anonymous
export const RegisterUser = async (values, onSubmitProps) => {
    if (values.profilePicture) {
      const imgUrl = await uploadToCloudinary(values.profilePicture);
      values.profilePicture = imgUrl;
    }
  
    try {
      const response = await instance.post(
        `${httpsApiCode}api/auth/register`,
        values
      );
      return response;
    } catch (error) {
      console.error("Unsuccessful registration", error);
    }
  };
  
  //login with credentials api call anonymous
  export const LoginUser = async (values, onSubmitProps) => {
    try {
      const response = await instance.post(
        `${httpsApiCode}api/auth/login`,
        values
      );
  
      return response;
    } catch (error) {
      console.error("Unsuccessful login", error);
    }
  };
  
  //login with refreshToken api call anonymous
  export const LoginRefresh = async () => {
    try {
      const response = await instance.get(
        `${httpsApiCode}api/auth/login/token`
      );
  
      return response;
    } catch (error) {
      console.error("Restricted! Log in with credentials", error);
    }
  };
  
  //logout to delete refreshToken api call anonymous
  export const LogoutRefresh = async () => {
    try {
      const response = await instance.get(`${httpsApiCode}api/auth/logout`);
      store.dispatch(setLogout({}));
      return response;
    } catch (error) {
      console.error("Log in with credentials", error);
      store.dispatch(setLogout({}));
      window.location.href = "/auth/login";
    }
  };

//Authorized api calls
//For Authorized calls there will be the need of JWT token and the RefreshToken which is HTTPS only
//Instance for being able to manage the token states
const instance = axios.create({
    baseURL: httpsApiCode,
    withCredentials: true,
  });
  
  instance.interceptors.response.use(
    (response) => {
      return response;
    },
    async function (error) {
      const originalRequest = error.config;
  
      if (error.response.status === 401 && !originalRequest._retry) {
        originalRequest._retry = true;
  
        try {
          const res = await LoginRefresh();
  
          if (res.status === 200) {
            // Dispatch the action here
            store.dispatch(
              setLogin({
                user: {
                  username: res.data.Username,
                  email: res.data.Email,
                  fName: res.data.FirstName,
                  lName: res.data.LastName,
                  pfp: res.data.ProfilePicture,
                  role: res.data.Role,
                  id: res.data.Id,
                },
              })
            );
            return instance(originalRequest);
          }
        } catch (err) {
          console.log("refresh token expired");
          await LogoutRefresh();
          // Add this line to redirect to your login page
          window.location.href = "/auth/login";
        }
      }
      return Promise.reject(error);
    }
  );
  