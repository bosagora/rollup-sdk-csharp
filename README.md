# Rollup SDK

C#에서 사용될 Rollup SDK


## 설치

```shell
git clone https://github.com/bosagora/rollup-sdk-csharp.git
```

## SDK 사용하기
SDK의 namespace는 BOSagora.Rollup.BlockChain 입니다. 
따라서 아래 코드를 제일 상단에 추가하여야 합니다.

```csharp
namespace BOSagora.Rollup.BlockChain;
```

## Transaction

|     | Rollup           | TheNine | Description     |
|-----|------------------|---------|-----------------|
| 1   | sequence         |         | 순번              |
| 2   | trade_id         | ID      | 거래아이디           |
| 3   | user_id          | MbKey   | 고객키             |
| 4   | state            | State   | 거래구분            |
| 5   | amount           | Amount  | 거래금액            |
| 6   | timestamp        | Time    | 거래발생시각          |
| 7   | exchange_user_id | MbHash  | 고객 해시코드         |
| 8   | exchange_id      | ApiKey  | APIKEY, 거래소 구분키 |
| 9   | signer           |         | 서명자의 이더리움주소    |
| 10  | signature        |         | 서명              |

### 순번 - sequence
이 값은 모든 트랜잭션들간에 고유한 값을 가져야 한다. 롤업서버로 전달될 때 순차적으로 1씩 증가된 값을 가져야 한다.  
롤업서버는 예상되는 순번과 다른 값을 전달받으면 오류(417)를 전달한다.


### 거래금액 - amount
이 값은 BigInteger로 처리된다.  
토큰은 소숫점자리를 18개를 가지고 있다. 그리고 그것을 소수부로 표시하지 않고 모두 정수로 처리한다.  
1토큰은 블록체인에 저장될 때, 1000000000000000000 로 저장된다.  
따라서 0.05120156 일 때는 51201560000000000 
이것을 쉽게 할 수 있도록 SDK에는 Amount 라는 클래스를 추가히였습니다. 사용법은 [단위테스트](RollupSDKTest/UnitTest1.cs)를 확인해 주세요


### 서명자의 주소 - signer
서명을 하기 위해서는 이더리움의 계정또는 비밀키를 가지고 있어야 한다.
비밀키를 생성하는 방법은 다양한 방법이 있다. 가장 쉬운 방법은 메타마스크 지갑에서 추출하는 것이다.
관련 문서 들을 참조해 주세요
https://docs.nethereum.com/en/latest/accounts/

### 트랜잭션 생성하기

```csharp
var tx = new Transaction(
    0,
    "12345678",
    "0x064c9Fc53d5936792845ca58778a52317fCf47F2",
    "0",
    Amount.Make("1.2345678").Value,
    1668044556,
    "997DE626B2D417F0361D61C09EB907A57226DB5B",
    "a5c19fed89739383");
```

### 서명하기
사용법은 [단위테스트](RollupSDKTest/UnitTest1.cs)를 확인해 주세요
관련 문서 들을 참조해 주세요
https://docs.nethereum.com/en/latest/nethereum-signing-messages/

```csharp
const string privateKey = "0xf6dda8e03f9dce37c081e5d178c1fda2ebdb90b5b099de1a555a658270d8c47d";
tx.Sign(privateKey);
```

### JSON 문자열로 추출하기

```csharp
var json = tx.ToJSON();
```
