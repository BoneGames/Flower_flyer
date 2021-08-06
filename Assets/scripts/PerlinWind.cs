
using UnityEngine;
using UnityEngine.UI;

public class PerlinWind : MonoBehaviour
{
    public Vector2 windSpeed;
    public int width = 256;
    public int height = 256;
    public float x, y;
    public float scale;
    public Vector2 offset_NS, offset_EW;
    public float windBase;
    public Text speedDisplay;
    public Transform spinner;
    public float delay;
    public float lastChange;
    Image windArrow;
    public AudioSource aS;
    public ParticleSystem dustStorm;
    ParticleSystem.VelocityOverLifetimeModule vel;
    ParticleSystem.EmissionModule emission;

    private void Awake()
    {
        vel = dustStorm.velocityOverLifetime;
        emission = dustStorm.emission;
        //rock = FindObjectOfType<Wacki.IndentSurface.IndentActor>();
        windArrow = spinner.GetComponentInChildren<Image>();
    }

    private void Start()
    {
        x = y = 0;
        offset_NS = new Vector2(Random.Range(0, 99999), Random.Range(0, 99999));
        offset_EW = new Vector2(Random.Range(0, 99999), Random.Range(0, 99999));



        //Renderer renderer = GetComponent<Renderer>();
        //renderer.material.mainTexture = GenerateTexture();
    }
    public float windDelta;
    void Update()
    {
        if (Time.time > lastChange + delay)
        {
            windSpeed = windBase * PerlinSample(x+=windDelta , y+= windDelta);

            // send movement
            //if(!rock.moveDisabled)
            //    rock.ApplyWind(windSpeed);

            // UI
            //float angle = Vector2.Angle(new Vector2(rock.forward.x, rock.forward.z), windSpeed);
            //float angle = Vector2.SignedAngle(new Vector2(rock.forward.x, rock.forward.z), windSpeed);
            //if(windSpeed.y < 0)
            //{
            //    angle = 360 - angle;
            //}
            //spinner.eulerAngles = new Vector3(0, 0, angle);
            //speedDisplay.text = windSpeed.magnitude.ToString() + " km/h";
            windArrow.transform.localScale = new Vector3(1, 1, 1) * windSpeed.magnitude;
            aS.volume = windSpeed.magnitude * windAdjust;
            lastChange += delay;

            DustStorm(windSpeed);
        }
    }

    void DustStorm(Vector2 windDir)
    {
        vel.xMultiplier = windDir.x * windSpeedVisual;
        vel.zMultiplier = windDir.y * windSpeedVisual;

        emission.rateOverTimeMultiplier = windDir.magnitude * 200;
    }
    public float windSpeedVisual;
    public float windAdjust;
    //Texture2D GenerateTexture()
    //{
    //    Texture2D tex = new Texture2D(width, height);

    //    for (int x = 0; x < width; x++)
    //    {
    //        for (int y = 0; y < height; y++)
    //        {
    //            Color color = CalculateColor(x, y);
    //            tex.SetPixel(x, y, color);
    //        }
    //    }
    //    tex.Apply();
    //    return tex;
    //}

    Vector2 PerlinSample(float x, float y)
    {
        return new Vector2(Mathf.PerlinNoise((float)x / width * scale + offset_NS.x, (float)y / height * scale + offset_NS.y) -0.5f,
                            Mathf.PerlinNoise((float)x / width * scale + offset_EW.x, (float)y / height * scale + offset_EW.y) - 0.5f);
    }
}
