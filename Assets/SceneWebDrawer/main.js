import * as THREE from 'three';
import * as core from './objects_management';

const renderer = new THREE.WebGLRenderer();
document.body.appendChild( renderer.domElement );
renderer.setSize( window.innerWidth, window.innerHeight );

const scene = new THREE.Scene();

const camera = new THREE.PerspectiveCamera( 90, window.innerWidth / window.innerHeight, 0.1, 1000 );

await core.createScene(scene, camera);

function animate() {
    //console.log(scene);
    renderer.render( scene, camera );
}

renderer.setAnimationLoop( animate );

