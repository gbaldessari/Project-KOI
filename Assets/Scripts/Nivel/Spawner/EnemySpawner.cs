using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private float cronometro = 0;
    public float inicio0 = 4;
    public float inicio1 = 12;
    public float inicio2 = 12;
    public float inicio3 = 10;
    public float inicio4 = 10;
    public float inicio5 = 10;
    public float inicio6 = 10;
    public float inicio7 = 10;
    public float inicio8 = 10;
    public float inicio9 = 10;
    public float inicio10 = 10;

    public Spawner spawner0;
    public Spawner spawner1;
    public Spawner spawner2;
    public Spawner spawner3;
    public Spawner spawner4;
    public Spawner spawner5;
    public Spawner spawner6;
    public Spawner spawner7;
    public Spawner spawner8;
    public Spawner spawner9;

    public BossContainer boss;

    void FixedUpdate()
    {
        cronometro += Time.deltaTime;

        if (cronometro >= inicio0)
        {
            spawner0.Inicio();
        }

        if (spawner0.Final() == true && cronometro >= inicio1 + inicio0)
        {
            spawner1.Inicio();
        }

        if (spawner1.Final() == true && cronometro >= inicio2 + inicio1 + inicio0)
        {
            spawner2.Inicio();
        }

        if (spawner2.Final() == true && cronometro >= inicio3 + inicio2 + inicio1 + inicio0)
        {
            spawner3.Inicio();
        }

        if (spawner3.Final() == true && cronometro >= inicio4 + inicio3 + inicio2 + inicio1 + inicio0)
        {
            spawner4.Inicio();
        }

        if (spawner4.Final() == true && cronometro >= inicio5 + inicio4 + inicio3 + inicio2 + inicio1 + inicio0)
        {
            spawner5.Inicio();
        }

        if (spawner5.Final() == true && cronometro >= inicio6 + inicio5 + inicio4 + inicio3 + inicio2 + inicio1 + inicio0)
        {
            spawner6.Inicio();
        }

        if (spawner6.Final() == true && cronometro >= inicio7 + inicio6 + inicio5 + inicio4 + inicio3 + inicio2 + inicio1 + inicio0)
        {
            spawner7.Inicio();
        }

        if (spawner7.Final() == true && cronometro >= inicio8 + inicio7 + inicio6 + inicio5 + inicio4 + inicio3 + inicio2 + inicio1 + inicio0)
        {
            spawner8.Inicio();
        }

        if (spawner8.Final() == true && cronometro >= inicio9 + inicio8 + inicio7 + inicio6 + inicio5 + inicio4 + inicio3 + inicio2 + inicio1 + inicio0)
        {
            spawner9.Inicio();
        }

        if (spawner9.Final() == true && cronometro >= inicio10 + inicio9 + inicio8 + inicio7 + inicio6 + inicio5 + inicio4 + inicio3 + inicio2 + inicio1 + inicio0)
        {
            boss.gameObject.SetActive(true);
            boss.IniciarEncuentro();
        }
    }
}
