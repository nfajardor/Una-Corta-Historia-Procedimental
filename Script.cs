using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script : MonoBehaviour
{

    Vector3 origen = new Vector3(0,0,0);
    Camera cam;
    GameObject ob;
    GameObject pMorado;
    GameObject magoA;
    GameObject magoR;
    GameObject magoV;
    GameObject magoY;    
    Transform amt;          //Componente de transformacion del altar morado
    AnimationCurve amac;    //Animation curve del altar morado
    float t0;
    float t1;
    float t2;
    float t3;
    float t4;
    float t5;
    float t6;
    AnimationCurve cam1;
    AnimationCurve cam2z;
    AnimationCurve cam2y;
    AnimationCurve cam2x;
    AnimationCurve cam3;
    AnimationCurve cam4z;
    AnimationCurve cam4y;
    AnimationCurve cam4x;
    AnimationCurve cam4yc;
    AnimationCurve cam4zc;
    AnimationCurve cam5;
    AnimationCurve cam6;
    AnimationCurve cam7x;
    AnimationCurve cam7y;
    bool plan;
    bool plan2;
    Quaternion rotY;
    Quaternion rotX;


    // Start is called before the first frame update
    Matrix4x4 temp;
    void Start()
    {     
        
        //Creacion de colores usados
        Color moradoOscuro = RGBToPercent(new Vector3(130,9,217));
        Color gris = RGBToPercent(new Vector3(93,92,97));
        Color grisClaro = RGBToPercent(new Vector3(115,149,174));
        Color negro = Color.black;
        Color azul = RGBToPercent(new Vector3(13,163,255));
        Color rojo = RGBToPercent(new Vector3(255,29,0));
        Color verde = RGBToPercent(new Vector3(20,166,43));
        Color amarillo = RGBToPercent(new Vector3(255,229,0));
        Color cafe = RGBToPercent(new Vector3(100,0,0));

        //creacion del entorno
        float lado = 1000;
        CrearPiso(lado);
        List<GameObject> arboles = new List<GameObject>();  
        for(int i = -990;i<1000;i+=20){
            for(int j =-990;j<1000;j+=20){
                bool b = false;
                float rand = Random.Range(1,11);
                if(rand<=5){
                    b = true;
                }
                int path = 30;
                if((i<path && i>-path)||(j<path && j>-path)){
                    b = false;
                }
                if(b){
                    GameObject arbol = CrearArbol();
                    arboles.Add(arbol);
                    arbol.transform.position += new Vector3(i,0,j);
                }
            }
        }
        float r1 = 10f/4;
        float r2 = 15f/4;
        float r3 = 22.5f/4;
        float r4 = 30f/4;
        float h1 = 10f/4;
        float h2 = 10f/4;
        GameObject altarA = VerticesAltar(new Vector3(500,0,0), r1, r2, r3, r4, h1, h2, 128, grisClaro, azul, negro, 8, 0);
        GameObject altarR = VerticesAltar(new Vector3(-500,0,0), r1, r2, r3, r4, h1, h2, 128, grisClaro, rojo, negro, 8, 0);
        GameObject altarV = VerticesAltar(new Vector3(0,0,500), r1, r2, r3, r4, h1, h2, 128, grisClaro, verde, negro, 8, 0);
        GameObject altarY = VerticesAltar(new Vector3(0,0,-500), r1, r2, r3, r4, h1, h2, 128, grisClaro, amarillo, negro, 8, 0);
        magoA = crearMago(new Vector3(0,4,0), (r2+r3)/2,2*(h1+h2), azul, 64);
        magoA.transform.position = new Vector3(500,2,0);
        magoR = crearMago(new Vector3(0,4,0), (r2+r3)/2,2*(h1+h2), rojo, 64);
        magoR.transform.position = new Vector3(-500,2,0);
        magoV = crearMago(new Vector3(0,4,0), (r2+r3)/2,2*(h1+h2), verde, 64);
        magoV.transform.position = new Vector3(0,2,500);
        magoY = crearMago(new Vector3(0,4,0), (r2+r3)/2,2*(h1+h2), amarillo, 64);
        magoY.transform.position = new Vector3(0,2,-500);
        
        //creacion de elementos noo visibles al comienzo
        GameObject altarM = VerticesAltar(origen, 10f/2, 15f/2, 22.5f/2, 30f/2, 10f/2, 10f/2, 128, grisClaro, moradoOscuro, negro, 8, 0);
        amt = altarM.transform;
        amt.position = new Vector3(0,-30,0);
        pMorado = VerticesPlaneta(new Vector3(0,-10,0),1f,90,180,moradoOscuro);
        pMorado.transform.parent = pMorado.transform;
        
        //ajustes iniciales de camara
        ob = new GameObject();
        cam = Camera.main;
        cam.transform.position = new Vector3(0,80,-80);
        cam.transform.Rotate(new Vector3(35,0,0));
        cam.transform.parent = ob.transform;
        temp = cam.projectionMatrix;

        //declaracion de Keyframes clave
        t0 = 12;
        t1 = 15;
        t2 = 25;
        t3 = 35;
        t4 = 55;
        t5 = 65;
        t6 = 70;
        

        //Subir el altar morado
        amac = new AnimationCurve(new Keyframe(0,-22),new Keyframe(t0,0));
        amac.postWrapMode = WrapMode.ClampForever;

        //rotar la camara alrededor del altar
        cam1 = new AnimationCurve(new Keyframe(0,0), new Keyframe(t0,360));
        cam1.postWrapMode = WrapMode.ClampForever;

        //posicionar la camara frente al altar
        cam2z = new AnimationCurve(new Keyframe(t0,-80),new Keyframe(t1,-30));
        cam2y = new AnimationCurve(new Keyframe(t0,80),new Keyframe(t1,5));
        cam2y.postWrapMode = WrapMode.ClampForever;
        cam2z.postWrapMode = WrapMode.ClampForever;
        cam2x = new AnimationCurve(new Keyframe(t0,35),new Keyframe(t1,-35));
        cam2x.postWrapMode = WrapMode.ClampForever;

        //Elevar la esfera morada
        cam3 = new AnimationCurve(new Keyframe(t1,10),new Keyframe(t2,45));
        cam3.postWrapMode = WrapMode.ClampForever;

        //mover la camara hacia el altar verde
        cam4z = new AnimationCurve(new Keyframe(t2,0),new Keyframe(t3,500));
        cam4z.postWrapMode = WrapMode.ClampForever;
        cam4y = new AnimationCurve(new Keyframe(t2,0),new Keyframe(t3,0));
        cam4y.postWrapMode = WrapMode.ClampForever;
        cam4x = new AnimationCurve(new Keyframe(t2,-35),new Keyframe(t3,35));
        cam4x.postWrapMode = WrapMode.ClampForever;
        cam4zc = new AnimationCurve(new Keyframe(t2,-30),new Keyframe(t3,450));
        cam4zc.postWrapMode = WrapMode.ClampForever;
        cam4yc = new AnimationCurve(new Keyframe(t2,5),new Keyframe(t3,50));
        cam4yc.postWrapMode = WrapMode.ClampForever;

        //rotar la camara para enfocar todos los altares
        cam5 = new AnimationCurve(new Keyframe(t3,0),new Keyframe(t3+5,90),new Keyframe(t3+10,180),new Keyframe(t3+15,270),new Keyframe(t4,360));
        cam5.postWrapMode = WrapMode.ClampForever;

        //inclinar los cañones
        cam6 = new AnimationCurve(new Keyframe(t3,0), new Keyframe(t4,45));
        cam6.postWrapMode = WrapMode.ClampForever;

        //trayectoria del ataque
        cam7x = new AnimationCurve(new Keyframe(t4,(float)(500-10*System.Math.Cos(System.Math.PI/4))),new Keyframe((t4+t5)/2,100),new Keyframe(t5,0));
        cam7y = new AnimationCurve(new Keyframe(t4,(float)(5+10*System.Math.Sin(System.Math.PI/4))),new Keyframe((t4+t5)/2,500),new Keyframe(t5,45));
        cam7x.postWrapMode = WrapMode.ClampForever;
        cam7y.postWrapMode = WrapMode.ClampForever;

        //booleanos auxiliares
        plan = false;
        plan2 = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        //Elevar el altar morado y rotar la camara 
        if(Time.time<=t0){
            rotY = Quaternion.Euler(0,cam1.Evaluate(Time.time),0);
            ob.transform.rotation = Quaternion.Slerp(ob.transform.rotation, rotY,Time.deltaTime*5.0f);
            amt.position = new Vector3(0,(float)(amac.Evaluate(Time.time)),0);
        }
        //Posicionar la camara frente al altar
        if(Time.time>t0 && Time.time <= t1){
            cam.transform.position = new Vector3(0, cam2y.Evaluate(Time.time), cam2z.Evaluate(Time.time));
            rotX = Quaternion.Euler(cam2x.Evaluate(Time.time),0,0);
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, rotX,Time.deltaTime*5.0f);
        }
        //Elevar la esfera morada
        if(Time.time>t1 && Time.time<=t2){
            pMorado.transform.position = new Vector3(0,cam3.Evaluate(Time.time),0);
            
        }
        //Posiciona la camara frente al altar verde
        if(Time.time>t2 && Time.time<=t3){
            rotX = Quaternion.Euler(cam4x.Evaluate(Time.time),0,0);
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, rotX,Time.deltaTime*5.0f);
            cam.transform.position = new Vector3(0,cam4yc.Evaluate(Time.time), cam4zc.Evaluate(Time.time));
        }
        //Ajusta la esfera morada y rota la camara alrededor de todos los magos
        if(Time.time>t3 && Time.time <= t4){
            CambiarPlaneta();
            rotY = Quaternion.Euler(0,cam5.Evaluate(Time.time),0);
            ob.transform.rotation = Quaternion.Slerp(ob.transform.rotation, rotY,Time.deltaTime*5.0f);
            Quaternion rotA = Quaternion.Euler(0,0,cam6.Evaluate(Time.time));
            Quaternion rotR = Quaternion.Euler(0,0,-cam6.Evaluate(Time.time));
            Quaternion rotV = Quaternion.Euler(-cam6.Evaluate(Time.time),0,0);
            Quaternion rotYe = Quaternion.Euler(cam6.Evaluate(Time.time),0,0); 
            magoA.transform.rotation = Quaternion.Slerp(magoA.transform.rotation, rotA, Time.deltaTime*5f); 
            magoR.transform.rotation = Quaternion.Slerp(magoR.transform.rotation, rotR, Time.deltaTime*5f); 
            magoV.transform.rotation = Quaternion.Slerp(magoV.transform.rotation, rotV, Time.deltaTime*5f); 
            magoY.transform.rotation = Quaternion.Slerp(magoY.transform.rotation, rotYe, Time.deltaTime*5f);           
        }
        //Posiciona la camara 1 en la parte superior
        if(Time.time>t4 && Time.time<=t4+2){
            ReposicionarCamara1();
        }
        //Disparo de los ataques
        if(Time.time>t4 && Time.time<=t5){
            magoA.transform.GetChild(1).position = new Vector3(cam7x.Evaluate(Time.time),cam7y.Evaluate(Time.time),0);
            magoR.transform.GetChild(1).position = new Vector3(-cam7x.Evaluate(Time.time),cam7y.Evaluate(Time.time),0);
            magoV.transform.GetChild(1).position = new Vector3(0,cam7y.Evaluate(Time.time),cam7x.Evaluate(Time.time));
            magoY.transform.GetChild(1).position = new Vector3(0,cam7y.Evaluate(Time.time),-cam7x.Evaluate(Time.time));
        }
        //reposiciona la camara frente al altar
        if(Time.time>(t5-2)){
            ReposicionarCamara2();
        }
        //Elimina las esferas
        if(Time.time>t5 && Time.time<=t6){
            quitarAtaques();
        }
    }
    void CambiarPlaneta(){
        if(!plan){
            pMorado.transform.position = new Vector3(0,-50,0);
            pMorado = VerticesPlaneta(new Vector3(0,45,0),30f,90,180,RGBToPercent(new Vector3(130,9,217)));
            plan = true;
        }
    }
    void ReposicionarCamara1(){
        if(!plan2){
            cam.transform.position = new Vector3(0,900,0);
            cam.transform.Rotate(55,0,0);
            cam.projectionMatrix = NuevaPerspectiva(-12,12,-12,12,20,1000);
            plan2 = true;
        }
    }
    void ReposicionarCamara2(){
        if(plan2){
            cam.projectionMatrix = temp;
            cam.transform.Rotate(-120,0,0);
            cam.transform.position = new Vector3(0,5,-100);
            plan2 = false;
        }
    }
    void quitarAtaques(){
        if(plan){
            magoA.transform.GetChild(1).position = new Vector3(0,-20,0);
            magoR.transform.GetChild(1).position = new Vector3(0,-20,0);
            magoV.transform.GetChild(1).position = new Vector3(0,-20,0);
            magoY.transform.GetChild(1).position = new Vector3(0,-20,0);
            pMorado.transform.position = new Vector3(0,-100,0);
            plan = false;
        }
    }
    

    GameObject crearMago(Vector3 origen, float r, float h, Color color, int n){
        Vector3[] vert = new Vector3[2+2*n];
        vert[0] = PolarToCartesian(new Vector3(0, 0, h), origen);
        vert[1] = PolarToCartesian(new Vector3(0, 0, 0), origen);
        float delta = (float)(2*System.Math.PI/n);
        for(int i = 2;i<n+2;i++){
            float theta = i*delta;
            vert[i] = PolarToCartesian(new Vector3(r, theta, h), origen);
        }
        for(int i = n+2;i<2*n+2;i++){
            float theta = i*delta;
            vert[i] = PolarToCartesian(new Vector3(r, theta, 0), origen);
        } 
        GameObject baseMago;
        baseMago = new GameObject();
        GameObject cuerpoMago = triPlaneta(vert,2,n, Color.black);
        cuerpoMago.transform.parent = baseMago.transform;
        GameObject cabezaMago = VerticesPlaneta(origen, r, n, n, color);
        cabezaMago.transform.parent = baseMago.transform;
        cabezaMago.transform.position += new Vector3(0,h + 1.1f*r,0);
        return baseMago;
    }

    static Matrix4x4 NuevaPerspectiva(float left, float right, float bottom, float top, float near, float far)
    {
        float x = 2.0F * near / (right - left);
        float y = 2.0F * near / (top - bottom);
        float a = (right + left) / (right - left);
        float b = (top + bottom) / (top - bottom);
        float c = -(far + near) / (far - near);
        float d = -(2.0F * far * near) / (far - near);
        float e = -1.0F;
        Matrix4x4 m = new Matrix4x4();
        m[0, 0] = x;
        m[0, 1] = 0;
        m[0, 2] = a;
        m[0, 3] = 0;
        m[1, 0] = 0;
        m[1, 1] = y;
        m[1, 2] = b;
        m[1, 3] = 0;
        m[2, 0] = 0;
        m[2, 1] = 0;
        m[2, 2] = c;
        m[2, 3] = d;
        m[3, 0] = 0;
        m[3, 1] = 0;
        m[3, 2] = e;
        m[3, 3] = 0;
        return m;
    }
    void CrearPiso(float lado){

        Vector3[] v = new Vector3[4];
        float l = lado;
        v[0] = new Vector3(l,0,l);
        v[1] = new Vector3(l,0,-l);
        v[2] = new Vector3(-l,0,-l);
        v[3] = new Vector3(-l,0,l);
        int[] tri = new int[] {3,0,1,1,2,3};
        GameObject obj = new GameObject("obj", typeof(MeshFilter), typeof(MeshRenderer));
        Mesh mesh = new Mesh();
        obj.GetComponent<Renderer>().material.color = Color.green;
        obj.GetComponent<MeshFilter>().mesh = mesh;
        mesh.Clear();
        mesh.vertices = v;
        mesh.triangles = tri;
        mesh.RecalculateNormals();
        obj.transform.position -= new Vector3(0,0.02f,0);
    }
    
    /*
     *Crea un arbol
     */
    GameObject CrearArbol(){
        GameObject cyl = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        cyl.GetComponent<Renderer>().material.color = RGBToPercent(new Vector3(89,33,24));
        cyl.transform.localScale = new Vector3(cyl.transform.localScale.x*2, 8*cyl.transform.localScale.y,cyl.transform.localScale.z*2);
        float altura = cyl.transform.localScale.y;
        GameObject[] hojas = new GameObject[3];
        for(int i = 0; i<hojas.Length;i++){
            hojas[i] = crearHojas();
            hojas[i].transform.position += new Vector3(0,(float)(altura*(i+1)/4),0);
            hojas[i].transform.parent = cyl.transform;
        }
        return cyl;
    }
    /*
     * Convierte el vector 3 de coordenadas polares en un Vector3 de coordenadas cartesianas y mueve las coordenadas al nuevo origen
     */
    Vector3 PolarToCartesian(Vector3 polar, Vector3 origen){
        float x = (float)((polar.x)*(System.Math.Cos(polar.y)));
        float z = (float)((polar.x)*(System.Math.Sin(polar.y)));
        float y = (float)(polar.z);
        return new Vector3(x +origen.x,y + origen.y,z + origen.z);
    }
    /*
     *Crea las hojas del arbool
     */
    GameObject crearHojas(){
        float radio = 3;
        float alto = 8;
        int divisiones = 360;
        Vector3[] vert = new Vector3[divisiones + 2];
        vert[0] = PolarToCartesian(new Vector3(0, 0, 0), origen);
        vert[1] = PolarToCartesian(new Vector3(0, 0, alto), origen);
        float delta = (float)(2*System.Math.PI/divisiones);
        for (int i = 2; i < divisiones+2; i++)
        {
            float theta = i*delta;
            vert[i] = PolarToCartesian(new Vector3(radio, theta, 0), origen);
        }

        int[] tri = new int[3*2*divisiones];

        int c = 0;
        int t = 2;
        for(int i = 0;i<divisiones;i++){

            tri[c] = 0;
            c++;
            tri[c] = t;
            c++;
            t++;
            if(t>(divisiones+1)){
                t= 2;
            }
            tri[c] = t;
            c++;
        }

        t = divisiones+1;
        for(int i = 0;i<divisiones;i++){
            tri[c] = 1;
            c++;
            tri[c] = t;
            c++;
            t--;
            if(t==1){
                t=divisiones+1;
            }
            tri[c] = t;
            c++;
        }
        Color color = RGBToPercent(new Vector3(14,61,10));
        return dibujarMesh(vert, tri, color);
    }

    GameObject dibujarMesh(Vector3[] vert, int[] tri, Color color) {
        GameObject obj = new GameObject("obj", typeof(MeshFilter), typeof(MeshRenderer));
        Mesh mesh = new Mesh();
        obj.GetComponent<Renderer>().material.color = color;
        obj.GetComponent<MeshFilter>().mesh = mesh;
        mesh.Clear();
        mesh.vertices = vert;
        mesh.triangles = tri;
        mesh.RecalculateNormals();
        return obj;
    }
    /*
     *Crea un color con base a un vector 3 con los valores RGB
     */
    Color RGBToPercent(Vector3 colores){
        Vector3 p = new Vector3((float)(colores.x/255),(float)(colores.y/255),(float)(colores.z/255));
        return new Color(p.x,p.y,p.z);
    }

    /*
     * Cuadra los vertices del altar
     */
    GameObject VerticesAltar(Vector3 origen, float r1, float r2, float r3, float r4, float h1, float h2, int n, Color color, Color colorPrincipal, Color colorSecundario, int velas, int id) {
        Vector3[] vert = new Vector3[4*n+2];
        vert[0] = PolarToCartesian(new Vector3(0,0,h1+h2),origen);
        vert[1] = PolarToCartesian(new Vector3(0,0,h2),origen);
        float delta = (float)(2*System.Math.PI/n);
        
        //Vertices r1
        for (int i = 2; i < (n+2); i++)
        {
            vert[i] = PolarToCartesian(new Vector3(r1,i*delta,h1+h2), origen);
        }
        //Vertices r3
        for (int i = n+2; i < (2*n+2); i++)
        {
            vert[i] = PolarToCartesian(new Vector3(r3,i*delta,h2), origen);
        }
        //Vertices r2
        for (int i = (2*n+2); i < (3*n+2); i++)
        {
            vert[i] = PolarToCartesian(new Vector3(r2,i*delta,h2), origen);
        }
        //Vertices r4
        for (int i = (3*n+2); i < (4*n+2); i++)
        {
            vert[i] = PolarToCartesian(new Vector3(r4,i*delta,0), origen);
        }

       GameObject altar = triAltar(vert, color, n);
       GameObject tapete1 = verticesTapeteCircular(origen, r3, n, colorSecundario,h1+0.002f);
       GameObject tapete2 = verticesTapeteCircular(origen, (r3+r2)/2, n, colorPrincipal,h1+0.004f);
       GameObject tapete3 = verticesTapeteCircular(origen, r1, n, colorSecundario, h1+h2+0.002f);
       GameObject tapete4 = verticesTapeteCircular(origen, r1*0.9f, n, colorPrincipal,h1+h2+0.004f);
       GameObject tapete5 = verticesTapeteCircular(origen, r1*0.5f, n, colorSecundario, h1+h2+0.006f);

       //Tapetes del piso
       GameObject tapete6 = verticesTapete(origen, 2*r4, colorPrincipal, 0.016f);
       GameObject tapete7 = verticesTapete2(origen, 2*r4, colorSecundario, 0.012f);
       GameObject tapete8 = verticesTapeteCircular(origen, 2*r4, n, colorPrincipal, 0.008f);
       GameObject tapete9 = verticesTapeteCircular(origen, (2*r4 + r4/6), n, colorSecundario, 0.004f);
       tapete1.transform.parent = altar.transform;
       tapete2.transform.parent = altar.transform;
       tapete3.transform.parent = altar.transform;
       tapete4.transform.parent = altar.transform;
       tapete5.transform.parent = altar.transform;
       tapete6.transform.parent = altar.transform;
       tapete7.transform.parent = altar.transform;
       tapete8.transform.parent = altar.transform;
       tapete9.transform.parent = altar.transform;
       float rp = (r2+r3)/2;
       float rv = 0.25f*(r3-r2);
       float h = h1*0.9f;

       //crearVelas(origen,rp,rv,h,h2,colorPrincipal,n,velas, altar);


       return altar;
    }
    /*
     * Crea los vertices de un tapete circular
     */
    GameObject verticesTapeteCircular(Vector3 origen, float radio, int n, Color color, float altura){
        Vector3[] vert = new Vector3[n+1];
        vert[0] = PolarToCartesian(new Vector3(0,0,altura), origen);
        float delta = (float)(2*System.Math.PI/n);
        for(int i = 1;i<=n;i++){
            vert[i] = PolarToCartesian(new Vector3(radio, i*delta, altura), origen);
        }
        return triTapeteCircular(vert, color);
    }
    /*
     * Crea los triangulos del tapete circular
     */
    GameObject triTapeteCircular(Vector3[] vert, Color color){
        int[] tri = new int[3*(vert.Length-1)];
        int j = vert.Length-1;
        int t = 1;
        for(int i = 0;i<3*(vert.Length-1);i++)
        {
            if(i%3==0){
                tri[i] = 0;
            }
            else if(i%3==1){
                tri[i] = j;
                j--;
            }
            else{
                if(j==(0)){
                    j = vert.Length-1;
                }
                tri[i] = j;
                //Debug.Log("Agregado triangulo " + t + ": " + tri[i-2] + "("+vert[tri[i-2]] +"), " + tri[i-1] + "("+vert[tri[i-1]] +"), " + tri[i] + "("+vert[tri[i]] +")");
                t++;
            }
        }
        return dibujarMesh(vert, tri, color);
    }
    /*
     * Cuadra los vertices de un tapete en el piso de la escena, en el centro, y los guarda en un arreglo de Vector3 llamado vert. Tambien pasa el color
     */
    GameObject verticesTapete(Vector3 origen, float lado, Color color, float elevacion){
        Vector3[] vert = new Vector3[4];
        vert[0] = (new Vector3(-lado/2,elevacion,-lado/2)) + origen;
        vert[1] = (new Vector3(lado/2,elevacion,-lado/2)) + origen;
        vert[2] = (new Vector3(-lado/2,elevacion,lado/2)) + origen;
        vert[3] = (new Vector3(lado/2,elevacion,lado/2)) + origen;
        return triangulosTapete(vert, color);
    }
    /*
     * Crea los vertices de un Segundo Tapete más grande que el anterior
     */
    GameObject verticesTapete2(Vector3 origen, float lado, Color color, float elevacion){
        Vector3[] vert = new Vector3[4];
        vert[0] = (new Vector3(-lado,elevacion,0)) + origen;
        vert[1] = (new Vector3(0,elevacion,-lado)) + origen;
        vert[2] = (new Vector3(0,elevacion,lado)) + origen;
        vert[3] = (new Vector3(lado,elevacion,0)) + origen;
        return triangulosTapete(vert, color);
    } 
     /*
     * Toma los vertices del tapete y guarda en un arreglo de enteros, tri, los triangulos para formar el tapete. Tambien pasa el color
     */
    GameObject triangulosTapete(Vector3[] vert, Color color){
        int[] tri = new int[6];
        tri[0] = 0;
        tri[1] = 2;
        tri[2] = 1;
        tri[3] = 1;
        tri[4] = 2;
        tri[5] = 3;
        return dibujarMesh(vert, tri, color);
    }
     /*
     * Dibuja los Triángulos de un altar.
     */
    GameObject triAltar(Vector3[] vert, Color color, int n){
        int[] tri = new int[3*6*n];

        //triangulos r1
        int j = n+1;
        int t = 0;
        for(int i = 0;i<3*n;i++){
            if(i%3==0){
                tri[i]=0;
            }else if(i%3==1){
                tri[i] = j;
                j--;
            }else{
                if(j==1){
                    j = n+1;
                }
                tri[i] = j;
                t++;
            }
        }

        //triangulos r3
        j = 2*n+1;
        for(int i = 3*n;i<6*n;i++){
            if(i%3==0){
                tri[i]=1;
            }else if(i%3==1){
                tri[i] = j;
                j--;
            }else{
                if(j==(n+1)){
                    j = 2*n+1;
                }
                tri[i] = j;
                t++;
            }
        }

        //triangulos r1-r2
        //Base en r3
        int inf = 2*n+2;
        int sup = 2;
        for(int i = 6*n;i<9*n;i++){
            if(i%3==0){
                tri[i] = inf;
                inf++;
            }else if(i%3==1){
                tri[i] = sup;
                sup++;
            }else{
                if(inf==(3*n+2)){
                    inf = 2*n+2;
                }
                tri[i] = inf;
                t++;
            }
        }
        //Base en r1
        sup = n+1;
        inf = 3*n+1;
        for(int i = 9*n;i<12*n;i++){
            if(i%3==0){
                tri[i] = sup;
                sup--;
            }else if(i%3==1){
                tri[i] = inf;
                inf--;
            }else{
                if(sup==1){
                    sup = n+1;
                }
                tri[i] = sup;
                t++;
            }
        }

        //Triangulos r3-r4
        //Base en piso
        inf = 3*n+2;
        sup = n+2;
        for(int i = 12*n;i<15*n;i++){
            if(i%3==0){
                tri[i] = inf;
                inf++;
            }else if(i%3==1){
                tri[i] = sup;
                sup++;
            }else{
                if(inf==(4*n+2)){
                    inf = 3*n+2;
                }
                tri[i] = inf;
                t++;
            }
        }
        //Base en r1
        sup = 2*n+1;
        inf = 4*n+1;
        for(int i = 15*n;i<18*n;i++){
            if(i%3==0){
                tri[i] = sup;
                sup--;
            }else if(i%3==1){
                tri[i] = inf;
                inf--;
            }else{
                if(sup==(n+1)){
                    sup = 2*n+1;
                }
                tri[i] = sup;
                t++;
            }
        }
        
        return dibujarMesh(vert, tri, color);
    }
     /*
     * Crea las velas de los altares
     */
    void crearVelas(Vector3 origen, float rp, float rv, float h, float elevacion, Color llama, int n, int cantidad, GameObject altar){
        Color cera = RGBToPercent(new Vector3(228,235,241));
        float delta = (float)(2*System.Math.PI/cantidad);
        float rv2 = rv/5;
        float h2 = h/5;
        for(int i = 0;i<cantidad;i++){

            Vector3 posActual = PolarToCartesian(new Vector3(rp,delta*i,elevacion), origen);
            GameObject cuerpoVela = verticesCilindro(posActual,n, rv, h, cera);

            GameObject mechaVela = verticesCilindro((posActual+=new Vector3(0,h,0)),n,rv2,h2,new Color(0,0,0));
            GameObject llamaVela = VerticesPlaneta((posActual+=new Vector3(0,h2,0)), (rv+rv2)/2, n, n, llama);

            cuerpoVela.transform.parent = altar.transform;
            mechaVela.transform.parent = altar.transform;
            llamaVela.transform.parent = altar.transform;
        }
    }
    GameObject verticesCilindro(Vector3 origen, int n, float r, float h, Color color){
        Vector3[] vert = new Vector3[2+2*n];
        vert[0] = PolarToCartesian(new Vector3(0, 0, h), origen);
        vert[1] = PolarToCartesian(new Vector3(0, 0, 0), origen);
        float delta = (float)(2*System.Math.PI/n);
        for(int i = 2;i<n+2;i++){
            float theta = i*delta;
            vert[i] = PolarToCartesian(new Vector3(r, theta, h), origen);
        }
        for(int i = n+2;i<2*n+2;i++){
            float theta = i*delta;
            vert[i] = PolarToCartesian(new Vector3(r, theta, 0), origen);
        }
        return triCilindro(vert,n,color);
    }
    GameObject triCilindro(Vector3[] vert, int n, Color color){
        int[] tri = new int[12*n];

        int j = n+1;
        for(int i = 0;i<n*3;i++){
            if(i%3==0){
                tri[i]=0;
            }else if(i%3==1){
                tri[i] = j;
                j--;
            }else{
                if(j==1){
                    j=n+1;
                }
                tri[i] = j;
            }
        }
        
        j = n+2;
        for(int i=3*n;i<6*n;i++){
            if(i%3==0){
                tri[i] = 1;
            }else if(i%3==1){
                tri[i] = j;
                j++;
            }else{
                if(j==(2*n+2)){
                    j=n+2;
                }
                tri[i] = j;
            }
        }

        int inf = n+2;
        int sup = 2;
        for(int i = 6*n;i<9*n;i++){
            if(i%3==0){
                tri[i] = inf;
                inf++;
            }else if(i%3==1){
                tri[i] = sup;
                sup++;
            }else{
                if(inf == 2*n+2){
                    inf = n+2;
                }
                tri[i] = inf;
            }
        }

        sup = n+1;
        inf = 2*n+1;
        for(int i = 9*n;i<12*n;i++){
            if(i%3==0){
                tri[i] = sup;
                sup--;
            }else if(i%3==1){
                tri[i] = inf;
                inf--;
            }else{
                if(sup==1){
                    sup = n+1;
                }
                tri[i] = sup;
            }
        }
        return dibujarMesh(vert, tri, color);
    }
    GameObject VerticesPlaneta(Vector3 origen, float r, int np, int nt, Color color){
        Vector3[] vert = new Vector3[2+nt*(np-1)];
        vert[0] = SphericalToCartesian(new Vector3(r,0,0), origen);
        vert[1] = SphericalToCartesian(new Vector3(r,0,(float)System.Math.PI), origen);
        float dp = (float)((System.Math.PI)/np);
        float dt = (float)((2*System.Math.PI)/nt);
        int c = 2;
        for(int i = 0;i<np-1;i++){
            float phi = (i+1)*dp;
            for(int j = 0;j<nt;j++){
                float theta = j*dt;
                vert[c] = SphericalToCartesian(new Vector3(r,theta,phi), origen);
                c++;
            }
        } 
       return triPlaneta(vert,np, nt, color);
    }
    GameObject triPlaneta(Vector3[] vert, int np, int nt, Color color) {
        int[] tri = new int[6*nt*(np-1)];

        //Triangulos de la parte de arriba de la esfera
        int j = nt+1;
        for(int i=0;i<3*nt;i++){
            if(i%3==0){
                tri[i] = 0; 
            }else if(i%3==1){
                tri[i] = j;
                j--;
            }else{
                if(j==1){
                    j=nt+1;
                }
                tri[i] = j;
            }
        }
        int t = 3*nt;
        //Triangulos centrales
        for(int p = 0;p<(np-2);p++){
            //Base Abajo
            int inf = (p+1)*nt+2;
            int sup = p*nt+2;
            for(int i= 0; i<3*nt ;i++){
                if(t%3==0){
                    tri[t] = inf;
                    inf++;
                    t++;
                }else if(t%3==1){
                    tri[t] = sup;
                    sup++;
                    t++;
                }else{
                    if(inf==(2+nt*(p+2))){
                        inf = (p+1)*nt+2;
                    }
                    tri[t] = inf;
                    t++;
                }
            }
            sup = (p+1)*nt+1;
            inf = (p+2)*nt+1;
            for(int i = 0; i<3*nt;i++){
                if(t%3==0){
                    tri[t] = sup;
                    t++;
                    sup--;
                }else if(t%3==1){
                    tri[t] = inf;
                    inf--;
                    t++;
                }else{
                    if(sup == nt*p+1){
                        sup = (p+1)*nt+1;
                    }
                    tri[t] = sup;
                    t++;
                }
            }
            //Base Arriba
        }

        j = 2+nt*(np-2);
        for(int i = 3*nt*(2*np-3);i<(6*nt*(np-1));i++){
            if(i%3==0){
                tri[i] = 1;
            }else if(i%3==1){
                tri[i] = j;
                j++;
            }else{
                if(j==(2+nt*(np-1))){
                    j = 2+nt*(np-2);
                }
                tri[i] = j;
            }
        }
        return dibujarMesh(vert, tri, color);
    }
     /*
     *Convierte el Vector3 de coordenadas esfericas en un Vector3 de coordenadas cartesianas. esferica = (r, t, p).
     */
    Vector3 SphericalToCartesian(Vector3 sph, Vector3 orgn){
        float r1 =(float)((sph.x)*System.Math.Cos((System.Math.PI/2)-sph.z));
        float x = (float)((r1)*System.Math.Cos(sph.y));
        float y = (float)((sph.x)*System.Math.Cos(sph.z));
        float z = (float)((r1)*System.Math.Sin(sph.y));
        x += orgn.x;
        y += orgn.y;
        z += orgn.z;
        return new Vector3(x,y,z);
    }
}
