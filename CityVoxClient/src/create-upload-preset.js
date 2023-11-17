require('dotenv').config();
const cloudinary= require('cloudinary').v2;

cloudinary.api.create_upload_preset({
    name:'profile_pic',
    tags: 'profile, face, pics',
    folder: 'profilePics',
    allowed_formats:'jpg, png, jpeg',
})
.then(uploadResult => console.log(uploadResult))
.catch(error=> console.error(error));