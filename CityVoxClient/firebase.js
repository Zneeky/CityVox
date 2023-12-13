// Import the functions you need from the SDKs you need
import { initializeApp } from "firebase/app";
import { getAnalytics } from "firebase/analytics";
// TODO: Add SDKs for Firebase products that you want to use
// https://firebase.google.com/docs/web/setup#available-libraries

// Your web app's Firebase configuration
// For Firebase JS SDK v7.20.0 and later, measurementId is optional
const firebaseConfig = {
  apiKey: "AIzaSyBH_l-B183lKwVrtmUBnt_e1uAuurpi7do",
  authDomain: "cityvoxclient.firebaseapp.com",
  projectId: "cityvoxclient",
  storageBucket: "cityvoxclient.appspot.com",
  messagingSenderId: "715343953696",
  appId: "1:715343953696:web:ba600a9264d677cbc4a145",
  measurementId: "G-L56STFV4K3"
};

// Initialize Firebase
const app = initializeApp(firebaseConfig);
const analytics = getAnalytics(app);