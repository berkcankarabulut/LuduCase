using UnityEngine;
using UnityEditor;// #if UNITY_EDITOR içerisine alınması gerekir. Build esnasında derlemede hata verecektir.
using System.Collections.Generic;

// Class'in yapılandırması doğru değil. AI ile ilgili herşeyi belli ki bu class barındıracak. Attack ,Move v.b. ve class okunabilirlikten ve modülerlikten uzaklaşacak.
// Bunun yerine detaylı bir yapı için behaviorTree kullanılabilir. Yada FiniteStatePattern ile ilerlenebilir.
public class EnemyAI : MonoBehaviour
{
    public List<Transform> waypoints; //Bunun private'a çekilip Serialiazeable yapılması daha sağlıklı olur. Dışardan erişime kapalı olması daha sağlıklı. 
    public float _speed = 5f; //Private değişkenlerde "_" kullanılır. C# kurallarına daha uygun olur.
    private int currentWaypoint = 0; // Burada değişken private o yüzden başına "_" koyulmalı. C# kurallarına daha uygun olur.

    // Private'a çekildikten sonra en azından Awake veya Start metodunda waypoints listesinin boş olup olmadığı kontrol edilmeli.

#if UNITY_EDITOR
    /*
     * Burada herhangi bir tetiklenme olmadığı için methodun silinmesi doğru olucaktır. Boşuna işlem maliyeti yapıyor.
     * Ayrıca unity workflow'undaki bir method'u bu şekilde Unity_Editor'de kullanmak doğru değildir.
     * Böyle bişey yapılmak isteniyorsa Start Methodunun içine "UNITY_EDITOR" yazılması daha doğru olur.
     */
    private void Start() {}
    
#endif

    void Update()
    {
        // if (waypoints == null || waypoints.Count == 0) return;
        
        Transform closesttarget = null; 
        /*
         * camelCase kullanımına uygun değil. "closestTarget" olmalı.
         * currentWaypoint değişkeni tanımlanmış ama hiç kullanılmıyor - waypoint'leri sırayla takip etmek için kullanılabilir        
        */
        float minDistance = Mathf.Infinity;
        
        /*
         * Her Frame'de en yakın waypoint'i hesaplıyor.
         * Aslında waypoint'ler arasında bir yol takibi yapmıyor.
         * Her zaman en yakın waypoint'e doğru hareket ediyor. 
         */
        foreach (var point in waypoints)
        {
            //waypoints'lerin null kontrol'u yapılması sağlıklı olucaktır.
            // Eksik: Waypoint'in null olma durumu kontrol edilmiyor
            // if (point == null) continue;
            
            float distance = Vector3.Distance(transform.position, point.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closesttarget = point;
            }
        }

        if (closesttarget != null)
        {
            // posA ve posB vector2 tipinde ama transform'a atama yapılırken Vector3'e yapılıyor. Vector2'de Z olmadığı için 0 gelecektir. Bu XYZ eksenli oyunlarda sorun çıkaracaktır.
            Vector2 posA = closesttarget.position;
            Vector2 posB = transform.position; 
            Vector3 direction = (posA - posB).normalized;
            transform.position += direction * _speed * Time.deltaTime;
            
            // Hedef noktaya varıldığının kontrolü yapılmıyor
            // Örneğin: if (Vector3.Distance(transform.position, closesttarget.position) < 0.1f) { ... }
        } 
    }
    
}