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

//Call to get the regions
export const GetRegions = async (token) => {
    try {
      const response = await instance.get("api/map/regions", {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
      return response.data;
    } catch (err) {
      console.log(err);
    }
  };

  export const GetMunicipalities = async (token, regionId) => {
    try {
      const response = await instance.get(`api/map/municipalities/${regionId}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
      return response.data;
    } catch (err) {
      console.log(err);
    }
  };
 //Call to get User's approved issue so they can create posts
  export const GetApprovedIssuesForUser = async (token, userId) => {
    try {
      const headers = {
        Authorization: `Bearer ${token}`,
      };
  
      // Start all three requests in parallel.
      const [response1, response2, response3] = await Promise.all([
        instance.get(`api/reports/valid/users/${userId}`, { headers }),
        instance.get(`api/emergencies/valid/users/${userId}`, { headers }),
        instance.get(`api/infIssues/valid/users/${userId}`, { headers }),
      ]);
  
      // Combine the results of the three responses into one array.
      let issueArray = [
        ...response1.data.$values,
        ...response2.data.$values,
        ...response3.data.$values,
      ];
  
      return issueArray; // return the combined array
    } catch (err) {
      console.log(err);
      return null; // or handle the error in a way suitable for your application
    }
  };
  //Call to create a Post
  export const CreatePost = async (token, postData) => {
    try {
      const response = await instance.post(`api/posts`, postData, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
  
      return response.data;
    } catch (err) {
      console.log(err);
    }
  };

//Call to create a formal Post only for representatives
export const CreateFormalPost = async (token, postData) => {
    try {
      const response = await instance.post(`api/posts/formal`, postData, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
  
      return response.data;
    } catch (err) {
      console.log(err);
    }
  };

  //Call to get posts by municipalityId
export const GetPostsByMuni = async (token, muniId) => {
  try {
    const response = await instance.get(`api/posts/municipalities/${muniId}`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
    return response.data.$values;
  } catch (err) {
    console.log(err);
  }
};

//Call to get formal posts by municipalityId
export const GetFormalPostsByMuni = async (token, muniId) => {
  try {
    const response = await instance.get(
      `api/posts/formal/municipalities/${muniId}`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
    return response.data.$values;
  } catch (err) {
    console.log(err);
  }
};

//Call to create a comment
export const CreateComment = async (token, commentDto) => {
  try {
    const response = await instance.post(`api/posts/comments`, commentDto, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
    return response.data.$values;
  } catch (err) {
    console.log(err);
  }
};

//Call to create a vote
export const CreateUpVote = async (token, postId) => {
  try {
    const response = await instance.post(
      `api/posts/vote/${postId}`,
      {}, //  body of the request
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
    return response;
  } catch (err) {
    console.log(err);
  }
};

//Call to delete a vote
export const DeleteUpVote = async (token, postId) => {
  try {
    const response = await instance.delete(`api/posts/vote/${postId}`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
    return response;
  } catch (err) {
    console.log(err);
  }
};