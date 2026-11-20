# 야시장 수호전 — Cloud Build PoC

Unity Fellowship 전용 별도 게임 **야시장 수호전(Stallkeepers of Hanyang)**의 클라우드 빌드 증명용 최소 프로젝트.
풀 게임(기획서 `../HANYANG-NIGHT-MARKET-DESIGN.md` v0.1: 카드 12종·10웨이브·적 6종+보스)이 아니라,
**파이프라인 증명용 작은 부분집합**만 들어 있다.

## PoC 범위 (이것만 증명한다)

- 밤 야시장 배경 (카메라 단색 배경 + 절차적 쿼드)
- 포장마차 토큰 1개: 떡볶이 (클릭 배치)
- 적 1마리: 잡귀, 웨이포인트 6개를 따라 이동
- 클릭 배치: 녹색 셀 1개를 클릭하면 떡볶이 포장마차가 놓인다
- 승/패 없음 — PoC 목표는 빌드 파이프라인 + 이동·배치 루프 증명뿐이다

## 파일 구성

```
poc/
├── README.md / VERSION-NOTES.md
├── .gitignore
├── Packages/manifest.json          # 빈 dependencies — Unity가 내장 모듈을 자동 채움
├── ProjectSettings/
│   ├── ProjectVersion.txt          # 6000.3.28f1
│   ├── ProjectSettings.asset       # 회사 Aldol Studio / 제품 Stallkeepers of Hanyang, 1920x1080
│   ├── EditorBuildSettings.asset   # Main.unity 1개만 빌드 대상으로 등록
│   └── TagManager.asset            # 최소 (태그 없음, Default 정렬 레이어만)
├── Assets/
│   ├── Scenes/Main.unity           # 카메라 + GameManager + 웨이포인트 6개 + 배치 셀 1개
│   ├── Scripts/
│   │   ├── GameManager.cs          # 길 그리기·적 스폰·상태 표시
│   │   ├── EnemyMover.cs           # 웨이포인트 이동
│   │   ├── StallPlacer.cs          # OnMouseDown 클릭 배치
│   │   └── PocVisuals.cs           # 절차적 쿼드 그리기 (바이너리 에셋 0개)
│   └── Editor/WebGLBuild.cs        # 선택 사항 — GameCI는 이 파일을 쓰지 않음
└── .github/workflows/build.yml     # game-ci/unity-builder@v4, WebGL, 6000.3.28f1
```

모든 비주얼은 코드로 만든 색 쿼드다. 텍스처·스프라이트·모델·오디오 파일이 없어서,
이 프로젝트는 텍스트 파일만으로 이뤄져 있다. Editor가 처음 열면
Graphics/Quality/Input 같은 나머지 설정 파일을 기본값으로 자동 생성한다.

## 클라우드 빌드 실행 방법 (레포 생성 후, 부모 에이전트 단계)

**이 폴더만으로는 빌드가 돌지 않는다. 아래는 아직 안 된 일이다.**

1. GitHub에 Private 레포를 새로 만든다 (예: `yajang-defense`). — **레포 생성은 부모 에이전트가 GitHub 웹사이트에서 한다. 이 작업에서는 만들지 않았다.**
2. 이 `poc/` 폴더 내용을 레포 루트에 푸시한다 (`.github/` 포함).
3. 레포 Settings → Secrets and variables → Actions에 넣는다:
   - `UNITY_LICENSE` — Unity Personal 라이선스 파일 내용 (1회 수동 활성화로 발급, 알돌 Unity 계정 단계)
   - `UNITY_EMAIL`, `UNITY_PASSWORD` — 알돌 Unity 계정 (채팅에 적지 않는다)
4. `main` 브랜치에 푸시하거나 Actions 탭에서 수동 실행한다.
5. 성공 기준: 워크플로 녹색 + `WebGL-PoC` artifact 다운로드 + 브라우저에서 첫 프레임 +
   적이 길을 따라 이동하고, 녹색 셀 클릭 시 떡볶이 포장마차가 놓이는 것.

## 검증 상태 (2026-11-20, 이 VM에서)

- **확인됨:**
  - GameCI Docker 이미지 `unityci/editor:ubuntu-6000.3.28f1-webgl-4.0.0` 실재 (Docker Hub API, 2026-11-18 푸시, 7,587,806,768 bytes, digest는 VERSION-NOTES.md 참조). 그래서 6000.0.23f1 폴백은 쓰지 않았다.
  - C# 파일 구조 검사 (Python): 중괄호/괄호 균형, 세미콜론·클래스·메소드 구조, Unity API 사용 범위 (아래 참조). **C# 컴파일러가 이 VM에 없어서 실제 컴파일은 못 했다.**
  - YAML 구조 검사 (Python): `.unity`/`.asset` 파일 파싱, 모든 `{fileID: ...}` 참조가 같은 파일 안에 존재하는지, MonoBehaviour의 script GUID가 해당 `.cs.meta`와 일치하는지, GUID 중복 없음.
  - 외부 URL 0건, 크레덴셜 0건, 다른 게임(요괴합전·럭키룰렛·보스힐러·샌드스타)에서 복사한 파일 0건 — 전수 grep으로 확인.
  - GitHub 연결 확인: 계정 `aldol2678`, MCP `connected`.
- **미확인 (실제 빌드가 돌아야 안다):**
  - 클라우드 빌드 자체가 아직 한 번도 안 돌았다 (레포 없음, 시크릿 없음).
  - 씬 YAML·PlayerSettings가 Editor에서 경고 없이 열리는지.
  - WebGL 첫 프레임·이동·클릭이 실제로 동작하는지.
  - GitHub Actions 분 잔량 (아래 블로커 참조).

## 남은 블로커 (첫 클라우드 빌드 전, 순서대로)

1. **레포 생성** — GitHub 웹사이트에서 Private 레포 생성 (부모 에이전트 단계, 이 작업에서는 안 함).
2. **GitHub App 선택** — Muse GitHub App에 새 레포를 설치/선택해야 푸시·읽기가 된다 (부모 에이전트 단계).
3. **라이선스 시크릿** — `UNITY_LICENSE`·`UNITY_EMAIL`·`UNITY_PASSWORD`를 GitHub Secrets에 등록. Personal 라이선스 1회 수동 활성화는 알돌 Unity 계정 단계이며, 값은 절대 채팅에 적지 않는다.
4. **Actions 분** — **확인 불가.** GitHub MCP의 읽기 도구에는 빌링·Actions 사용량 조회가 없어서 현재 잔량을 확인할 수 없었다. 2026-10-20 보고 기준 포함 3,000분이 100% 소진됐고 11/1 리셋 예정, $0 상한 결정은 대기 중이었다. 첫 빌드 전 GitHub Settings → Billing에서 잔량·상한을 직접 확인할 것. Unity 빌드는 1회당 수십 분이 들 수 있다.
5. **Unity 버전 일치** — GameCI는 `6000.3.28f1`을 쓴다. 나중에 로컬 Editor를 쓰는 사람이 있으면 같은 버전(6000.3.28f1 LTS)으로 맞출 것.
