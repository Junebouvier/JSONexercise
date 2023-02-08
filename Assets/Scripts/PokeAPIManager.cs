using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PokeAPIManager : MonoBehaviour
{
    [SerializeField] 
    private RawImage PokeImage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void GetPokemonClick()
    {
        StartCoroutine(GetPokemon());
    }

    IEnumerator GetPokemon()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://pokeapi.co/api/v2/pokemon/");
        yield return www.Send();

        if (www.isNetworkError)
        {
            Debug.Log("Network error: " + www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            if (www.responseCode == 200)
            {
                PokemonList pokemons = JsonUtility.FromJson<PokemonList>(www.downloadHandler.text);
                PokemonListInfo pokemonsinfo = JsonUtility.FromJson<PokemonListInfo>(www.downloadHandler.text);
                //Debug.Log(pokemonsinfo.count);
                //Debug.Log(pokemonsinfo.next);

                foreach (Pokemon pokemon in pokemons.results)
                {
                    Debug.Log("Name: " + pokemon.name);
                    //StartCoroutine(DownloadImage(pokemon.image));
                    
                    UnityWebRequest wwwstats = UnityWebRequest.Get(pokemon.url);
                    yield return wwwstats.Send();
                    
                    if (wwwstats.isNetworkError) Debug.Log("Network error: " + www.error);

                    else
                    {
                        Debug.Log(wwwstats.downloadHandler.text);

                        if (wwwstats.responseCode == 200)
                        {
                            PokemonDataList pokedata = JsonUtility.FromJson<PokemonDataList>(wwwstats.downloadHandler.text);

                            foreach (PokemonData pokestats in pokedata.types)
                            {
                                Debug.Log("Name: " + pokestats.slot);
                            }
                        }
                    }
                    
                }
            } 
            
            else
            {
                string mensaje = "Status: " + www.responseCode;
                mensaje += "\ncontent-type: " + www.GetResponseHeader("content-type");
                mensaje += "\nError : " + www.error;
                Debug.Log(mensaje);
            }
        }
    }
}
[System.Serializable]
public class PokemonList
{
    //public PokemonListInfo info;
    public List<Pokemon> results;
    
}

public class PokemonDataList
{
    public List<PokemonData> types;
}
[System.Serializable]
public class PokemonListInfo
{
    public int count;
    public string next;
    public string previous;

}
[System.Serializable]
public class Pokemon
{
    public string name;
    public string url;
}


[System.Serializable]
public class PokemonData
{
    public string slot;
}
