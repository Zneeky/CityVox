import axios from "axios";
import {
  CLOUDINARY_UPLOAD_PRESET,
  CLOUDINARY_UPLOAD_URL,
  httpsApiCode,
} from "./consts";

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
    const response = await instance.get(`${httpsApiCode}api/auth/login/token`);

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

//Call to get the regions
export const GetRegions = async () => {
  try {
    const response = await instance.get("api/map/regions");
    return response.data;
  } catch (err) {
    console.log(err);
  }
};

export const GetMunicipalities = async (regionId) => {
  try {
    const response = await instance.get(`api/map/municipalities/${regionId}`);
    return response.data;
  } catch (err) {
    console.log(err);
  }
};

//Call to create a new report
export const CreateReport = async (formData) => {
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
    const response = await instance.post(`api/reports`, appropriateDTO);
    return response.data;
  } catch (err) {
    console.log(err);
  }
};

//Call to get a report
export const GetReport = async (reportId) => {
  try {
    const response = await instance.get(`api/reports/${reportId}`);
    return response.data;
  } catch (err) {
    console.log(err);
  }
};

//Call to update a report
export const UpdateReport = async (formData) => {
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
      updateReportDto
    );
    return response.data;
  } catch (err) {
    console.log(err);
  }
};

//Call to get all reports by municipality
export const GetReportsByMunicipality = async (muniId) => {
  try {
    const response = await instance.get(`api/reports/municipalities/${muniId}`);
    return response?.data?.$values;
  } catch (err) {
    console.log(err);
  }
};

//Call to delete a reprot
export const DeleteReport = async (reportId) => {
  try {
    const response = await instance.delete(`api/reports/${reportId}`);
  } catch (err) {
    console.log(err);
  }
};

//Call to create an emergency
export const CreateEmergency = async (formData) => {
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
    const response = await instance.post(`api/emergencies`, appropriateDTO);
    return response.data;
  } catch (err) {
    console.log(err);
  }
};

//Call to get an emergency
export const GetEmergency = async (emergencyId) => {
  try {
    const response = await instance.get(`api/emergencies/${emergencyId}`);
    return response.data;
  } catch (err) {
    console.log(err);
  }
};

//Call to update an emergency
export const UpdateEmergency = async (formData) => {
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
      updateEmergecyDto
    );
    return response.data;
  } catch (err) {
    console.log(err);
  }
};

//Call to delete an emergency
export const DeleteEmergency = async (emergencyId) => {
  try {
    const response = await instance.delete(`api/emergencies/${emergencyId}`);
  } catch (err) {
    console.log(err);
  }
};

//Call to get all emergencies by municipality
export const GetEmergenciesByMunicipality = async (muniId) => {
  try {
    const response = await instance.get(
      `api/emergencies/municipalities/${muniId}`
    );
    return response?.data?.$values;
  } catch (err) {
    console.log(err);
  }
};

//Call to create a new InfIssue
export const CreateInfIssue = async (formData) => {
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
    const response = await instance.post(`api/infIssues`, appropriateDTO);
    return response.data;
  } catch (err) {
    console.log(err);
  }
};

//Call to get a InfIssue
export const GetInfIssue = async (infIssueId) => {
  try {
    const response = await instance.get(`api/infIssues/${infIssueId}`);
    return response.data;
  } catch (err) {
    console.log(err);
  }
};

//Call to update a InfIssue
export const UpdateInfIssue = async (formData) => {
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
      updateInfIssueDto
    );
    return response.data;
  } catch (err) {
    console.log(err);
  }
};
//Call to create an infIssue
export const DeleteInfIssue = async (infIssueId) => {
  try {
    const response = await instance.delete(`api/infIssues/${infIssueId}`);
  } catch (err) {
    console.log(err);
  }
};

//Call to get all infIssues by municipality
export const GetInfIssuesByMunicipality = async (muniId) => {
  try {
    const response = await instance.get(
      `api/infIssues/municipalities/${muniId}`
    );
    return response?.data?.$values;
  } catch (err) {
    console.log(err);
  }
};

//Call to get User's approved issue so they can create posts
export const GetApprovedIssuesForUser = async (userId) => {
  try {
    // Start all three requests in parallel.
    const [response1, response2, response3] = await Promise.all([
      instance.get(`api/reports/valid/users/${userId}`),
      instance.get(`api/emergencies/valid/users/${userId}`),
      instance.get(`api/infIssues/valid/users/${userId}`),
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
export const CreatePost = async (postData) => {
  try {
    const response = await instance.post(`api/posts`, postData);

    return response.data;
  } catch (err) {
    console.log(err);
  }
};

//Call to create a formal Post only for representatives
export const CreateFormalPost = async (postData) => {
  try {
    const response = await instance.post(`api/posts/formal`, postData);

    return response.data;
  } catch (err) {
    console.log(err);
  }
};

//Call to get posts by municipalityId
export const GetPostsByMuni = async (muniId) => {
  try {
    const response = await instance.get(`api/posts/municipalities/${muniId}`);
    return response.data.$values;
  } catch (err) {
    console.log(err);
  }
};

//Call to get formal posts by municipalityId
export const GetFormalPostsByMuni = async (muniId) => {
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
export const CreateComment = async (commentDto) => {
  try {
    const response = await instance.post(`api/posts/comments`, commentDto);
    return response.data.$values;
  } catch (err) {
    console.log(err);
  }
};

//Call to create a vote
export const CreateUpVote = async (postId) => {
  try {
    const response = await instance.post(`api/posts/vote/${postId}`);
    return response;
  } catch (err) {
    console.log(err);
  }
};

//Call to delete a vote
export const DeleteUpVote = async (postId) => {
  try {
    const response = await instance.delete(`api/posts/vote/${postId}`);
    return response;
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
