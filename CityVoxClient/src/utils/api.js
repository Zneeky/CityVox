import axios from "axios";
import {
  CLOUDINARY_UPLOAD_PRESET,
  CLOUDINARY_UPLOAD_URL,
  httpsApiCode,
} from "./consts";

import { setLogin, setLogout } from "../redux";
import { store } from "../redux/store";

//appUser object
const appUser = {
  accessToken: null,
  email: null,
  fName: null,
  lName: null,
  pfp: null,
  role: null,
  is: null,
};

//img upload to cloudinary.com anonymous
export const uploadToCloudinary = async (file) => {
  const formData = new FormData();
  formData.append("file", file);
  formData.append("upload_preset", CLOUDINARY_UPLOAD_PRESET);
  try {
    const response = await axios.post(`${CLOUDINARY_UPLOAD_URL}`, formData);

    const img = await response.data;
    return img.secure_url;
  } catch (error) {
    console.error("Error uploading image:", error);
  }
};

//API calls to OpenStreetMap.com overpass-turbo
const overpassApiUrl = "https://overpass-api.de/api/interpreter";

//API call for getting municipality boundaries
export const MunicipalityBoundaries = async (osmId) => {
  const query = `
    [out:json];
    (
      relation(${osmId});
    );
    out body;
    >;
    out skel qt;
    `;

  try {
    const response = await axios.post(
      overpassApiUrl,
      { data: query },
      {
        headers: {
          "Content-Type": "application/x-www-form-urlencoded",
        },
      }
    );
    return response;
  } catch (error) {
    console.error(error);
  }
};

//registration api call anonymous
export const RegisterUser = async (values, onSubmitProps) => {
  if (values.profilePicture) {
    const imgUrl = await uploadToCloudinary(values.profilePicture);
    values.profilePicture = imgUrl;
  }
  try {
    const response = await axios.post(
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
    const response = await axios.post(`${httpsApiCode}api/auth/login`, values);

    return response;
  } catch (error) {
    console.error("Unsuccessful login", error);
  }
};

//login with refreshToken api call anonymous
export const LoginRefresh = async (accessToken) => {
  try {
    const response = await axios.get(
      `${httpsApiCode}api/auth/login/refres?JwtToken=${accessToken}`
    );

    return response;
  } catch (error) {
    console.error("Restricted! Log in with credentials", error);
  }
};

//logout to delete refreshToken api call anonymous
export const LogoutRefresh = async () => {
  try {
    const response = await axios.get(`${httpsApiCode}api/auth/logout`);
    return response;
  } catch (error) {
    console.error("Log in with credentials", error);
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
        const accessTokenString = originalRequest.headers["Authorization"];
        const accessToken = accessTokenString.split(" ")[1];
        const res = await LoginRefresh(accessToken);

        if (res.status === 200) {
          originalRequest.headers[
            "Authorization"
          ] = `Bearer ${res.data.AccessToken}`;
          // Dispatch the action here
          store.dispatch(
            setLogin({
              user: {
                accessToken: res.data.AccessToken,
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

//Authorized calls
//Call to promote user to admin
export const PromoteToAdmin = async (token, username) => {
  try {
    const response = await instance.post(
      `api/users/admins`,
      JSON.stringify(username),
      {
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        },
      }
    );
    return response.data;
  } catch (err) {
    console.log(err);
  }
};

//Call to promote user to representative
export const PromoteToRepresentative = async (token, muniRepDto) => {
  try {
    const response = await instance.post(
      `api/users/representatives`,
      muniRepDto,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
    return response.data;
  } catch (err) {
    console.log(err);
  }
};

//Call to update state user
export const UpdateCurrentUser = async (token, updateUserDto) => {
  try {
    const response = await instance.patch(`api/users`, updateUserDto, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });

    if (response.status === 200) {
      // Dispatch the action here
      store.dispatch(
        setLogin({
          user: {
            accessToken: response.data.AccessToken,
            username: response.data.Username,
            email: response.data.Email,
            fName: response.data.FirstName,
            lName: response.data.LastName,
            pfp: response.data.ProfilePicture,
            role: response.data.Role,
            id: response.data.Id,
          },
        })
      );
    }
    return response;
  } catch (err) {
    console.log(err);
  }
};

//Call to get users count
export const GetUsersCount = async (token) => {
  try {
    const response = await instance.get(`api/users/count`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
    return response.data;
  } catch (err) {
    console.log(err);
  }
};

//Call to get users by page and count
export const GetUsers = async (token, page, count) => {
  try {
    const response = await instance.get(
      `api/users?page=${page}&count=${count}`,
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

//Call to get municipalities based on region Id
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

//Call to get all reports by municipality
export const GetReportsByMunicipality = async (token, muniId) => {
  try {
    const response = await instance.get(
      `api/reports/municipalities/${muniId}`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
    return response?.data?.$values;
  } catch (err) {
    console.log(err);
  }
};

//Call to get all requested reports
export const GetRequestedReports = async (token, page, count) => {
  try {
    const response = await instance.get(
      `api/reports/requests?page=${page}&count=${count}`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
    return response?.data?.$values;
  } catch (err) {
    console.log(err);
  }
};

//Call to get the count of all requested reports
export const GetRequestedReportsCount = async (token) => {
  try {
    const response = await instance.get(`api/reports/requests/count`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
    return response;
  } catch (err) {
    console.log(err);
  }
};

//Call to create a new report
export const CreateReport = async (token, formData) => {
  const appropriateDTO = {
    CreatorId: formData.creatorId,
    Title: formData.title,
    Description: formData.description,
    ImageUrl: formData.imageUrl,
    Latitude: parseFloat(formData.latitude),
    Longitude: parseFloat(formData.longitude),
    Address: formData.address,
    MunicipalityId: formData.municipalityId,
    Type: formData.issueType,
  };

  try {
    const response = await instance.post(`api/reports`, appropriateDTO, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
    return response.data;
  } catch (err) {
    console.log(err);
  }
};

//Call to get a report
export const GetReport = async (token, reportId) => {
  try {
    const response = await instance.get(`api/reports/${reportId}`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
    return response.data;
  } catch (err) {
    console.log(err);
  }
};

//Call to update a report
export const UpdateReport = async (token, formData) => {
  const updateReportDto = {
    Title: formData.Title,
    Description: formData.Description,
    ImageUrl: formData.ImageUrl,
    Type: formData.TypeValue,
    Status: formData.StatusValue,
  };
  try {
    const response = await instance.patch(
      `api/reports/${formData.Id}`,
      updateReportDto,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );

    return response.data;
  } catch (err) {
    console.log(err);
  }
};

//Call to delete a reprot
export const DeleteReport = async (token, reportId) => {
  try {
    const response = await instance.delete(`api/reports/${reportId}`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
  } catch (err) {
    console.log(err);
  }
};

//Call to create an emergency
export const CreateEmergency = async (token, formData) => {
  const appropriateDTO = {
    CreatorId: formData.creatorId,
    Title: formData.title,
    Description: formData.description,
    ImageUrl: formData.imageUrl,
    Latitude: parseFloat(formData.latitude),
    Longitude: parseFloat(formData.longitude),
    Address: formData.address,
    MunicipalityId: formData.municipalityId,
    Type: formData.issueType,
  };

  try {
    const response = await instance.post(`api/emergencies`, appropriateDTO, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
    return response.data;
  } catch (err) {
    console.log(err);
  }
};

//Call to get an emergency
export const GetEmergency = async (token, emergencyId) => {
  try {
    const response = await instance.get(`api/emergencies/${emergencyId}`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
    return response.data;
  } catch (err) {
    console.log(err);
  }
};

//Call to update an emergency
export const UpdateEmergency = async (token, formData) => {
  const updateEmergecyDto = {
    Title: formData.Title,
    Description: formData.Description,
    ImageUrl: formData.ImageUrl,
    Type: formData.TypeValue,
    Status: formData.StatusValue,
  };
  try {
    const response = await instance.patch(
      `api/emergencies/${formData.Id}`,
      updateEmergecyDto,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );

    return response.data;
  } catch (err) {
    console.log(err);
  }
};

//Call to delete an emergency
export const DeleteEmergency = async (token, emergencyId) => {
  try {
    const response = await instance.delete(`api/emergencies/${emergencyId}`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
  } catch (err) {
    console.log(err);
  }
};

//Call to get all emergencies by municipality
export const GetEmergenciesByMunicipality = async (token, muniId) => {
  try {
    const response = await instance.get(
      `api/emergencies/municipalities/${muniId}`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
    return response?.data?.$values;
  } catch (err) {
    console.log(err);
  }
};

//Call to get all requested emergencies
export const GetRequestedEmergencies = async (token, page, count) => {
  try {
    const response = await instance.get(
      `api/emergencies/requests?page=${page}&count=${count}`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
    return response?.data?.$values;
  } catch (err) {
    console.log(err);
  }
};

//Call to get the count of all requested reports
export const GetRequestedEmergenciesCount = async (token) => {
  try {
    const response = await instance.get(`api/emergencies/requests/count`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
    return response;
  } catch (err) {
    console.log(err);
  }
};

//Call to get all infIssues by municipality
export const GetInfIssuesByMunicipality = async (token, muniId) => {
  try {
    const response = await instance.get(
      `api/infIssues/municipalities/${muniId}`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
    return response?.data?.$values;
  } catch (err) {
    console.log(err);
  }
};

//Call to get all requested InfIssues
export const GetRequestedInfIssues = async (token, page, count) => {
  try {
    const response = await instance.get(
      `api/infIssues/requests?page=${page}&count=${count}`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
    return response?.data?.$values;
  } catch (err) {
    console.log(err);
  }
};

//Call to get the count of all requested InfIssues
export const GetRequestedInfIssuesCount = async (token) => {
  try {
    const response = await instance.get(`api/infIssues/requests/count`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
    return response;
  } catch (err) {
    console.log(err);
  }
};

//Call to create a new InfIssue
export const CreateInfIssue = async (token, formData) => {
  const appropriateDTO = {
    CreatorId: formData.creatorId,
    Title: formData.title,
    Description: formData.description,
    ImageUrl: formData.imageUrl,
    Latitude: parseFloat(formData.latitude),
    Longitude: parseFloat(formData.longitude),
    Address: formData.address,
    MunicipalityId: formData.municipalityId,
    Type: formData.issueType,
  };

  try {
    const response = await instance.post(`api/infIssues`, appropriateDTO, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
    return response.data;
  } catch (err) {
    console.log(err);
  }
};

//Call to get a InfIssue
export const GetInfIssue = async (token, infIssueId) => {
  try {
    const response = await instance.get(`api/infIssues/${infIssueId}`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
    return response.data;
  } catch (err) {
    console.log(err);
  }
};

//Call to update a InfIssue
export const UpdateInfIssue = async (token, formData) => {
  const updateInfIssueDto = {
    Title: formData.Title,
    Description: formData.Description,
    ImageUrl: formData.ImageUrl,
    Type: formData.TypeValue,
    Status: formData.StatusValue,
  };
  try {
    const response = await instance.patch(
      `api/infIssues/${formData.Id}`,
      updateInfIssueDto,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );

    return response.data;
  } catch (err) {
    console.log(err);
  }
};

//Call to create an infIssue
export const DeleteInfIssue = async (token, infIssueId) => {
  try {
    const response = await instance.delete(`api/infIssues/${infIssueId}`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
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
