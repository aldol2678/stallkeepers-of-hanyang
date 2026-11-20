# VERSION-NOTES — Unity 버전 선택 근거 (2026-11-20)

## 선택: Unity 6000.3.28f1 (LTS)

- 기획서(`../HANYANG-NIGHT-MARKET-DESIGN.md`)와 나루 PC 환경이 쓰던 버전과 동일하다.
- `ProjectSettings/ProjectVersion.txt`와 `.github/workflows/build.yml`의
  `unityVersion: 6000.3.28f1`이 이 버전을 가리킨다.

## 검증: GameCI Docker 이미지가 실재한다 (폴백 불필요)

Docker Hub API (`hub.docker.com/v2/repositories/unityci/editor/tags?name=6000.3.28f1`,
2026-11-20 12:38 UTC 조회) 결과:

- 태그: `ubuntu-6000.3.28f1-webgl-4.0.0` (동일 이미지의 별칭: `6000.3.28f1-webgl-4.0.0`,
  `ubuntu-6000.3.28f1-webgl-4`, `6000.3.28f1-webgl-4`)
- 상태: `active`, 마지막 푸시 `2026-11-18T10:23:14Z`, 마지막 풀 `2026-11-18T21:18:14Z`
- 크기: 7,587,806,768 bytes (약 7.1 GiB)
- 이미지 digest: `sha256:c7571d7fe4496a64deaa76ba509b4d15aee9cc0c1d2b35a0c14bbcd56c32a356`
- 태그 digest: `sha256:cfcf57c6ddedc38b5033ca621665d833b2f5f3bcf495ecd59e3a6c058513f1db`

GameCI `unity-builder@v4`는 `unityVersion` + `targetPlatform: WebGL`로 위 이미지를
자동 선택하므로, 워크플로에 `customImage`를 따로 적지 않았다.

## 폴백 후보 (쓰지 않음 — 기록만)

- `6000.0.23f1` (GameCI 문서 예시 버전): 태그 `ubuntu-6000.0.23f1-webgl-4.0.0`도
  실재한다 (2026-11-20 푸시, 7,107,021,748 bytes). 6000.3.28f1 이미지가 없어졌을 때만
  이 버전으로 바꾸고, 그때는 `ProjectVersion.txt`도 함께 바꿔야 한다.
- 6000.3.28f1 이미지가 확인됐으므로 폴백은 적용하지 않았다.

## 주의

- `ProjectVersion.txt`에는 `m_EditorVersion: 6000.3.28f1` 한 줄만 적었다.
  원래 파일의 두 번째 줄(`m_EditorVersionWithRevision`, 해시값)은 이 VM에서 알 수 없어
  임의의 해시를 적지 않았다. GameCI는 버전 줄만 읽고, Editor는 처음 열 때 해시를 채운다.
- `ProjectSettings.asset`은 최소 필드만 적었다 (회사·제품명·해상도·버전).
  나머지 설정(Graphics/Quality/Input 등)은 Editor가 처음 열 때 기본값으로 생성한다.
