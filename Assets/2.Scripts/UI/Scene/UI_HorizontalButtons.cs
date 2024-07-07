using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_HorizontalButtons : UI_Scene
{
    enum Buttons
    {
        UpgradeButton,
        SpawnButton,
        GambleButton
    }

    enum TMPs
    {
        GamblePriceTMP,
        SpawnPriceTMP
    }

    enum Texts
    {
        Spawn_Text
    }

    TextMeshProUGUI gameblePriceTMP;
    TextMeshProUGUI spawnPriceTMP;
    Text spawnText;

    private void Start() 
    {
        Bind<TextMeshProUGUI>(typeof(TMPs));
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.SpawnButton).gameObject.BindEvent(ClickSpawnButton);
        GetButton((int)Buttons.GambleButton).gameObject.BindEvent(ClickGambleSpawnButton);
        GetButton((int)Buttons.UpgradeButton).gameObject.BindEvent((p) => Managers.UI.ShowPopupUI<UI_UpgradePopup>());

        gameblePriceTMP = Get<TextMeshProUGUI>((int)TMPs.GamblePriceTMP);
        spawnPriceTMP = Get<TextMeshProUGUI>((int)TMPs.SpawnPriceTMP);
        spawnText = Get<Text>((int)Texts.Spawn_Text);

        Managers.Spawn.RegisterUI(UpdateUI);
        Managers.Upgrade.RegisterUpdateUI(UpdateSpawnUI);

        UpdateUI();
        UpdateSpawnUI();
    }



    private void UpdateUI()
    {
        gameblePriceTMP.text = Managers.Spawn.GetGambleSpawnPrice().ToString();
        spawnPriceTMP.text = Managers.Spawn.GetSpawnPrice().ToString();
    }

    private void UpdateSpawnUI()
    {
        spawnText.text = $"LV.{Managers.Object.Player.SpawnLevel} 생성";
    }

    private void ClickSpawnButton(PointerEventData data)
    {
        GameObject go = null;
        if (Managers.Spawn.Spawn(Managers.Object.Player, out go))
        {
            go.SetActive(false);

            Vector3 startWorldPos = UIToWorldPoint();

            // 랜덤한 방향과 거리를 계산하여 제어점을 설정합니다.
            Vector3 randomDir = Random.insideUnitCircle.normalized; // 랜덤 방향 벡터
            float randomDist = Random.Range(2f, 5f); // 랜덤 거리 범위 설정

            Vector3 controlPoint1 = startWorldPos + randomDir * randomDist + Vector3.up * 2f; // 첫 번째 제어점
            Vector3 controlPoint2 = Vector3.Lerp(startWorldPos, Vector3.zero, 0.5f) + Vector3.up * -2f; // 두 번째 제어점

            // 목표
            Vector3 endPosition = go.transform.position;

            // 시작 위치에서 종료 위치까지의 베지어 곡선을 생성합니다.
            StartCoroutine(AnimateUnitSpawn(startWorldPos, endPosition, go, controlPoint1, controlPoint2));
        }
    }

    private void ClickGambleSpawnButton(PointerEventData data)
    {
        GameObject go = null;
        if (Managers.Spawn.GambleSpawn(Managers.Object.Player, out go))
        {
            go.SetActive(false);

            Vector3 startWorldPos = UIToWorldPoint();

            // 랜덤한 방향과 거리를 계산하여 제어점을 설정합니다.
            Vector3 randomDir = Random.insideUnitCircle.normalized; // 랜덤 방향 벡터
            float randomDist = Random.Range(2f, 5f); // 랜덤 거리 범위 설정

            Vector3 controlPoint1 = startWorldPos + randomDir * randomDist + Vector3.up * 2f; // 첫 번째 제어점
            Vector3 controlPoint2 = Vector3.Lerp(startWorldPos, Vector3.zero, 0.5f) + Vector3.up * -2f; // 두 번째 제어점

            // 목표
            Vector3 endPosition = go.transform.position;

            // 시작 위치에서 종료 위치까지의 베지어 곡선을 생성합니다.
            StartCoroutine(AnimateUnitSpawn(startWorldPos, endPosition, go, controlPoint1, controlPoint2));
        }
    }

    // 버튼 애니메이션
    private IEnumerator AnimateUnitSpawn(Vector3 start, Vector3 end, GameObject go, Vector3 controlPoint1, Vector3 controlPoint2)
    {
        float elapsedTime = 0f;

        GameObject unit = Managers.Resource.Instantiate("Cube",pooling:true);

        while (elapsedTime < 0.5f)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / 0.5f;
            Vector3 position = Utils.BezierCurve(start, controlPoint1, controlPoint2, end, t);
            unit.transform.position = position;
            yield return null;
        }
        // 유닛 소환 후 추가 로직 처리
        go.SetActive(true);
        unit.transform.position = end;
        Managers.Resource.Destroy(unit);
    }



}
