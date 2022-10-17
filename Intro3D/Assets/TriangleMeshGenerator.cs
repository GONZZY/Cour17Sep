using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
public class TriangleMeshGenerator : MonoBehaviour
{
    // 1. Créer une structure Mesh 
    // 2. Modifier le mesh: I. (donner un tableau de sommets)
    //                      II. (donner un tableau qui décrit la connexion entre les sommets)
    // 3. donner le mesh au MeshFilter
    
    // ------------------------------------ AUTRES INFOS À SAVOIR -----------------------------------------
    // Chaque objet a son propre espace nommé Object Space. L'origine est son centre.

    // Les faces sont seulement visible d'un côté à cause du backface culling qui est utilisé pour optimisé
    
    // Convention: si l'on veut que lorsqu'on fait le truc de la main la normale vise comme notre pouce. Donc,
    //             que la face visible soit dans notre direction il faut aligner les connexions dans le sens
    //             HORAIRE.
    // -------------------------------------------------------------------------------------------------

    [SerializeField] private float height = 1;
    [SerializeField] private float width = 1;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 rotation;
    [SerializeField] private Vector3 scale;
    private void Awake()
    {
        // Définir dimension Triangle
        float halfHeight = height / 2;
        float halftWidth = width / 2;
        // 1. -->
        Mesh triangleMesh = new Mesh();
        
        // 2 -->
        // On peut aussi utiliser SetVertices()
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0f, halfHeight),
            new Vector3(halftWidth, -halfHeight),
            new Vector3(-halftWidth, -halfHeight),
            // new Vector3(-1.5f,-1.5f)

        };
        // En multipliant une matrice 4x4 par un vertex on peut le déplacer, tourner et scaler.
        Debug.Log(offset);
        Matrix4x4 translation = Matrix4x4.Translate(offset);
        Matrix4x4 rotationMatrix = Matrix4x4.Rotate(Quaternion.Euler(rotation));
        Matrix4x4 scaleMatrix = Matrix4x4.Scale(scale);
        Matrix4x4 trsMatrix = Matrix4x4.TRS(offset,Quaternion.Euler(rotation),scale );
        // Nous pouvons traiter plusieurs sommets en même temps dans des shaders, mais nous n'avons
        // pas le temps de voir ça.
        for (int i = 0; i < 3; ++i)
        { 
            // Manière normale
            //vertices[i] += offset;
            
            // Manière Matricielle
            // Ordre des modifications importante à cause produit matricielle n'est pas commutatif
            // Translation --> Rotation --> Scale 
            // Vector3 currentVertex = vertices[i]; 
            // currentVertex = translation.MultiplyPoint(currentVertex);
            //  currentVertex = rotationMatrix.MultiplyPoint(currentVertex);
            //  currentVertex = scaleMatrix.MultiplyPoint(currentVertex);
            //  vertices[i] = currentVertex;
             
             // ou on peut seulement faire 
             vertices[i] = trsMatrix.MultiplyPoint(vertices[i]);
        }
        
        
        triangleMesh.vertices = vertices;
        triangleMesh.triangles = new int[]
        {
            // il faut donner des paquets de 3 indices
            // Le 1er connecte au 2ème (0 à 1)
            // Le 2ème  connecte au 3ème (1 à 2)
            // Le 3ème connecte au 1er (2 à 0)
            0,1,2,
            
            // Face visible inversée -->
             0,2,1,
             //0,3,2
        };
        // 3 -->
        GetComponent<MeshFilter>().mesh = triangleMesh;
        
    }
}
