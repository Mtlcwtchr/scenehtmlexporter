import * as THREE from "three";
import {Color, Euler, MathUtils, Matrix4, Quaternion, Vector3} from "three";
import {randFloat} from "three/src/math/MathUtils";

let objects = [];

export async function createScene(scene, camera) {
    await fetch('SceneData.json')
        .then(response => response.json())
        .then(jsonResponse => createSceneInternal(scene, camera, jsonResponse));

    console.log(objects);
    console.log(camera);
}

function createSceneInternal( scene, camera, json )
{
    json.values.forEach( value => {
        if(value.type === 'SceneObject')
        {
            const obj = {
                name: value.name,
                parent: value.parent,
                matrix: TRS(PositionFromString(value.position), QuaternionFromEulerString(value.eulerAngles), Vector3FromString(value.scale)),
                children: []
            }
            objects.push(obj);
        }
        else if (value.type === 'Camera')
        {
            setCameraTransform( value, camera );
        }
    })

    objects.forEach( obj => {
        let parent = Find(obj.parent);
        if(parent) parent.children.push(obj);
    })

    objects.forEach(obj => createObject(obj, scene));

    objects.forEach(obj => {
        manageHierarchy(obj)
    })

    objects.forEach(obj => console.log(obj))
}

function TRS(pos, rot, scale)
{
    let matrix = new Matrix4();
    matrix.compose(pos, rot, scale);
    return matrix;
}

function Find(name)
{
    return objects.find(value => value.name === name);
}

function QuaternionFromEulerString(str)
{
    let euler = Vector3FromString(str);
    let rot = new Quaternion();
    rot.setFromEuler(new Euler(MathUtils.degToRad(-euler.x), MathUtils.degToRad(-euler.y), MathUtils.degToRad(euler.z), 'YXZ'));
    return rot;
}

function PositionFromString(str)
{
    let vector = Vector3FromString(str);
    return new Vector3(vector.x, vector.y, -vector.z);
}

function Vector3FromString(str)
{
    let split = str.split(',');
    let x = parseFloat(split[0].trim());
    let y = parseFloat(split[1].trim());
    let z = parseFloat(split[2].trim());
    return new Vector3(x, y, z);
}

function setCameraTransform(json, camera)
{
    const pos = PositionFromString(json.position);
    const rot = QuaternionFromEulerString(json.eulerAngles);

    let mat = TRS(pos, rot, new Vector3(1, 1, 1));

    camera.applyMatrix4(mat);
}

function createObject(obj, scene)
{
    let cube = createCube(scene);
    cube.applyMatrix4(obj.matrix);

    obj.mesh = cube;
}

function manageHierarchy(obj)
{
    obj.children.forEach( child => {
        child.mesh.material.color = obj.mesh.material.color;
        child.mesh.parent = obj.mesh;
    } )
}

function createCube(scene)
{
    const geometry = new THREE.BoxGeometry( 1, 1, 1);
    const material = new THREE.MeshNormalMaterial( { color: new Color(randFloat(0, 1), randFloat(0, 1), randFloat(0, 1)) } );
    const cube = new THREE.Mesh( geometry, material );

    scene.add( cube );
    return cube;
}