import React, { useRef, useEffect } from 'react';
import * as THREE from 'three';
import ThreeGlobe from "three-globe"
import countries from './custom.geo.json'
import{OrbitControls} from 'three/examples/jsm/controls/OrbitControls'

function ThreeScene() {
  const mountRef = useRef(null);

  

  useEffect(() => {
    var controls;
    // Scene setup
    
    const scene = new THREE.Scene();
    const camera = new THREE.PerspectiveCamera(100, window.innerWidth / window.innerHeight, 0.1, 1000);
    const renderer = new THREE.WebGLRenderer({antialias:true, aplha: true});
    renderer.setPixelRatio(window.devicePixelRatio);
    renderer.setSize(190, 150);
    renderer.setClearColor(0xffffff, 0);
    mountRef.current.appendChild(renderer.domElement);

    var ambientLight = new THREE.AmbientLight(0xbbbbb,0.3)
    scene.add(ambientLight)
  
  
    var dLight = new THREE.DirectionalLight(0xfffff,0.8);
    dLight.position.set(-800,2000,400);
    camera.add(dLight);
  
    var dLight1 = new THREE.DirectionalLight(0x8566cc,1);
    dLight1.position.set(-200,500,200);
    camera.add(dLight1);
  
    var dLight2 = new THREE.DirectionalLight(0x8566cc,0.5);
    dLight2.position.set(-200,500,200);
    camera.add(dLight2);
  
    camera.position.z= 400;
    camera.position.x=0;
    camera.position.y=0;
  
    scene.add(camera);

    scene.fog = new THREE.Fog(0x535ef3,400,2000);

    controls = new OrbitControls(camera, renderer.domElement)
    controls.enableDamping =true;
    controls.dynamicDampingFactor = 0.01;
    controls.enablePan = false;
    controls.minDistance = 200;
    controls.maxDistance = 500;
    controls.rotateSpeed = 0.8;
    controls.autoRotate = false;
  
    controls.minPolarAngle = Math.PI/3.5;
    controls.maxPolarAngle = Math.PI - Math.PI/3;
  
    //window.addEventListener("resize", onWindowResize,false);
    //document.addEventListener("mousemove", onMouseMove);
    //Globe 
    function initGlobe(){
        var Globe;
        Globe = new ThreeGlobe({
          waitForGlobeReady: true,
          animateIn: true,
        })
      
        .hexPolygonsData(countries.features)
        .hexPolygonResolution(3)
        .hexPolygonMargin(0.7)
        .showAtmosphere(true)
        .atmosphereColor("#3a228a")
        .atmosphereAltitude(0.25)
      
      
       
      
      
      
        Globe.rotateY(-Math.PI*(5/9));
        Globe.rotateZ(-Math.PI/6);
        const globeMaterial = Globe.globeMaterial();
        globeMaterial.color = new THREE.Color(0x3a228a);
        globeMaterial.emissive = new THREE.Color(0x220038);
        globeMaterial.emissiveIntensity = 0.1;
        globeMaterial.shininess = 0.7;
      
        scene.add(Globe);
      }


    // Animation loop
    const animate = function () {
        renderer.render(scene,camera);
        renderer.render(scene, camera);
        requestAnimationFrame(animate);
    };


    initGlobe();
    animate();

    // Cleanup
    return () => {
      mountRef.current.removeChild(renderer.domElement);
    };
  }, []);

  return <div ref={mountRef} />;
}

export default ThreeScene;