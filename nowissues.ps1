########################################################################################################################
# 스크립트 구조
# 가. 함수 정의부
# 나. 함수 호출부
# 
# 


########################################################################################################################
# 가. 함수 정의부
# 
# 
# 
# 

#function Convert-To-Markdown-PlainLink {
#    param (
#        [string]$LINK_TITLE,     # 링크명
#        [string]$LINK_URL        # 링크URL
#    )
#
#    if ($null -eq $LINK_TITLE) {
#        Write-Error "LINK_TITLE is required."
#        return
#    }
#    if ($null -eq $LINK_URL) {
#        Write-Error "LINK_URL is required."
#        return
#    }
#
#    return ("[" + $LINK_TITLE + "](" + $LINK_URL + ")")
#}
#
#function Convert-To-Markdown-SelectBoxLink {
#    param (
#        [string]$LINK_SELECTED,  # 선택여부
#        [string]$LINK_TITLE,     # 링크명
#        [string]$LINK_URL        # 링크URL
#    )
#    if ($LINK_SELECTED -eq $null) {
#        Write-Error "LINK_SELECTED is required."
#        return
#    }
#    if ($LINK_TITLE -eq $null) {
#        Write-Error "LINK_TITLE is required."
#        return
#    }
#    if ($LINK_URL -eq $null) {
#        Write-Error "LINK_URL is required."
#        return
#    }
#
#    return ("- [" + ($LINK_SELECTED -eq $true ? "x" : " ") + "] " + (Convert-To-Markdown-PlainLink -LINK_TITLE $LINK_TITLE -LINK_URL $LINK_URL))
#}

<#
.SYNOPSIS
    이 함수는 연결된 이슈를 검색하고 출력 파일에 기록합니다.

.DESCRIPTION
    이 함수는 GitHub GraphQL API를 사용하여 특정 이슈와 관련된 이슈를 검색하고, 
    이를 지정된 파일에 마크다운 형식으로 기록합니다.

.PARAMETER OWNER
    리포지토리 소유자의 이름입니다.

.PARAMETER REPONAME
    리포지토리의 이름입니다.

.PARAMETER issuenum
    검색할 이슈의 번호입니다.

.PARAMETER FILENAME
    출력 파일의 이름입니다. 기본값은 'issues.md'입니다.

.EXAMPLE
    Search-And-Print-ConnectedIssues -OWNER "octocat" -REPONAME "Hello-World" -issuenum 42 -FILENAME "output.md"
#>
function Search-And-Print-ConnectedIssues {
    param (
        [string]$OWNER,     # 소유자
        [string]$REPONAME,  # 리포지토리명
        [int]$issuenum,     # 이슈번호
        [string]$FILENAME    # 출력파일명
    )

    #-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
    # 필수값, 기본값 설정
    if ($OWNER -eq $null) {
        Write-Error "OWNER is required."
        return
    }
    if ($REPONAME -eq $null) {
        Write-Error "REPONAME is required."
        return
    }
    if ($issuenum -eq $null) {
        Write-Error "issuenum is required."
        return
    }
    if ($FILENAME -eq $null) {
        Write-Host "FILENAME is not set. Default value is 'issues.md'."
        $FILENAME = "issues.md"
    }
    
    #-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
    # 관련된 이슈 전체조회
    #
    # 
    # 1. closedByPullRequestsReferences : 연결된 풀 리퀘스트
    # 2. timelineItems - ConnectedEvent : 이 이슈가 참조한 이슈
    # 3. timelineItems - ReferencedEvent : 이 이슈를 참조한 이슈
    # 4. timelineItems - CrossReferencedEvent : 이슈간에 서로 참조된 이슈
    #     
    $ISSUE_DETAILS_STR = gh api graphql -F owner=$OWNER -F repo=$REPONAME -F issue=$issuenum -f query='
    query ($owner: String!, $repo: String!, $issue: Int!) {
        repository(owner: $owner, name: $repo) {
            issue(number: $issue) {
                url,
                assignees(first: 100) {
                    nodes {
                        login
                        name
                    }
                },
                title,
                number,
                closed,
                closedByPullRequestsReferences(first: 100) {
                    nodes {
                        number
                        title
                        closed
                        closedAt
                        url
                    }
                },
                participants(first: 100) {
                    nodes {
                        login
                        name
                    }
                },
                timelineItems(first: 100){
                    edges{
                        node{
                            ... on ConnectedEvent{
                                source{
                                    ... on Issue{
                                        labels(first: 100){
                                            nodes{
                                                name
                                            }
                                        }
                                        title
                                        number
                                        closed
                                        closedAt
                                        updatedAt
                                        url
                                        assignees(first: 100){
                                            nodes{
                                                login
                                                name
                                            }
                                        }
                                    }
                                }
                            }
                            ... on ReferencedEvent{
                                subject{
                                    ... on Issue{
                                        labels(first: 100){
                                            nodes{
                                                name
                                            }
                                        }
                                        title
                                        number
                                        closed
                                        closedAt
                                        updatedAt
                                        url
                                        assignees(first: 100){
                                            nodes{
                                                login
                                                name
                                            }
                                        }
                                    }
                                }
                            }
                            ... on CrossReferencedEvent{
                                source{
                                    ... on Issue{
                                        labels(first: 100){
                                            nodes{
                                                name
                                            }
                                        }
                                        title
                                        number
                                        closed
                                        closedAt
                                        updatedAt
                                        url
                                        assignees(first: 100){
                                            nodes{
                                                login
                                                name
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }'
    $ISSUE_DETAILS = $ISSUE_DETAILS_STR | ConvertFrom-Json
    

    #-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
    # 관련된 이슈 가공, 마크다운으로 출력
    #
    #
    # 1. 이슈
    
    (("- [" +($ISSUE_DETAILS.data.repository.issue.closed -eq "True" ? "x" : " ") + "] [" + ($ISSUE_DETAILS.data.repository.issue.title) + " #" + ($ISSUE_DETAILS.data.repository.issue.number) + "](" + ($ISSUE_DETAILS.data.repository.issue.url) + ") / " + "``" + $ISSUE_DETAILS.data.repository.issue.assignees.nodes[0].login + ":" + $ISSUE_DETAILS.data.repository.issue.assignees.nodes[0].name) + "``  ") | Out-File -FilePath $FILENAME -Append
    
    # 2. 풀 리퀘스트
    $ISSUE_DETAILS.data.repository.issue.closedByPullRequestsReferences.nodes | ForEach-Object {
        ("  - [" +($_.closed -eq "True" ? "x" : " ") + "] PULL REQUEST: [" + ($_.title) + "](" + ($_.url) + ")  ") | Out-File -FilePath $FILENAME -Append
    }

    # 3-1. 연결된 이슈 필터링, 'task' 라벨이 있는 이슈만
    $CONNECTED_ISSUES = $ISSUE_DETAILS.data.repository.issue.timelineItems.edges | Where-Object { 
        if ($null -ne $_.node.source){
            ($null -ne $_.node.source.closed) -and 
            ($_.node.source.number -ne $issuenum) -and
            ($_.node.source.labels.nodes.name -contains "task")
        }
        elseif ($null -ne $_.node.subject){
            ($null -ne $_.node.subject.closed) -and 
            ($_.node.subject.number -ne $issuenum) -and
            ($_.node.source.labels.nodes.name -contains "task")
        }
    } | Sort-Object -Property ($_.node.source.title)

    # 3-2. 연결된 이슈 출력
    $processedNumbers = @()
    $CONNECTED_ISSUES | ForEach-Object { 
        $connected = $_.node.source 
        if ($processedNumbers -notcontains $connected.number) {
            $processedNumbers += $connected.number
            ("  - [" +($connected.closed -eq "True" ? "x" : " ") + "] CONNECTED ISSUE: [" + ($connected.title) + " #" + ($connected.number) + "](" + ($connected.url) + ")/" + "``" + $connected.assignees.nodes[0].login + ":" + $connected.assignees.nodes[0].name) + "``  " | Out-File -FilePath $FILENAME -Append
        }
    }
}
    



########################################################################################################################
# 나. 함수 호출부
# 
# 
# 
# 

#-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
# 공통 영역
$start = Get-Date
Write-Host started at $start.ToString("yyyy-MM-dd HH:mm:ss")

$OWNER = "aliencube"
$REPONAME = "azure-openai-sdk-proxy"
$FILENAME = "issues.md"

Remove-Item -Path $FILENAME -Force
New-Item -Path $FILENAME -ItemType File


#-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
# 제목
Add-Content -Path $FILENAME -Value '# 진행 현황'
Add-Content -Path $FILENAME -Value ('> 생성일자: ' + (Get-Date -Format "yyyy-MM-dd HH:mm:ss").ToString() + '  ')
Add-Content -Path $FILENAME -Value ('> 스크립트: [https://gist.github.com/tae0y/6ca6fbab44ba60d72934cbbc107a06bb](https://gist.github.com/tae0y/6ca6fbab44ba60d72934cbbc107a06bb)')


#-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
# PR 현황
Add-Content -Path $FILENAME -Value `n`n
Add-Content -Path $FILENAME -Value '## PR 현황'
gh pr list --json title,number,labels,assignees,updatedAt `
 | jq -s -c '.[] | sort_by(.updatedAt) | reverse' `
 | jq -r --arg OWNER $OWNER --arg REPONAME $REPONAME '.[] | "- [ ] [\(.title)](https://github.com/\($OWNER)/\($REPONAME)/pull/\(.number)) / `@\(.assignees[0].login):\(.assignees[0].name)` / updated:`\(.updatedAt)`"' `
 | Out-File -Append -FilePath $FILENAME

Add-Content -Path $FILENAME -Value `n`n
Add-Content -Path $FILENAME -Value '## 팀별 진행현황 (updated 기준 내림차순)'

#-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
# 팀별 현황
#team-kim 🔑, team-oh 🙆‍♂️, team-park 🐧
Add-Content -Path $FILENAME -Value ''
Add-Content -Path $FILENAME -Value '### team-kim 🔑'
gh issue list --json title,number,labels,assignees,updatedAt --label 'team-kim' `
 | jq -s -c '.[] | sort_by(.updatedAt) | reverse' `
 | jq -r --arg OWNER $OWNER --arg REPONAME $REPONAME '.[] | "- [ ] [\(.title)](https://github.com/\($OWNER)/\($REPONAME)/issues/\(.number)) / `@\(.assignees[0].login):\(.assignees[0].name)`"' `
 | Out-File -Append -FilePath $FILENAME

Add-Content -Path $FILENAME -Value ''
Add-Content -Path $FILENAME -Value '### team-oh 🙆‍♂️'
gh issue list --json title,number,labels,assignees,updatedAt --label 'team-oh' `
 | jq -s -c '.[] | sort_by(.updatedAt) | reverse' `
 | jq -r --arg OWNER $OWNER --arg REPONAME $REPONAME '.[] | "- [ ] [\(.title)](https://github.com/\($OWNER)/\($REPONAME)/issues/\(.number)) / `@\(.assignees[0].login):\(.assignees[0].name)`"' `
 | Out-File -Append -FilePath $FILENAME

Add-Content -Path $FILENAME -Value ''
Add-Content -Path $FILENAME -Value '### team-park 🐧'
gh issue list --json title,number,labels,assignees,updatedAt --label 'team-park' `
 | jq -s -c '.[] | sort_by(.updatedAt) | reverse' `
 | jq -r --arg OWNER $OWNER --arg REPONAME $REPONAME '.[] | "- [ ] [\(.title)](https://github.com/\($OWNER)/\($REPONAME)/issues/\(.number)) / `@\(.assignees[0].login):\(.assignees[0].name)`"' `
 | Out-File -Append -FilePath $FILENAME


#-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
# 이슈 분배 현황
Add-Content -Path $FILENAME -Value `n`n
Add-Content -Path $FILENAME -Value '## 이슈현황'

# EPIC ISSUE
Add-Content -Path $FILENAME -Value '### EPIC 🚀'
gh issue list --json title,number,labels,updatedAt --label 'epic' `
 | jq -s -c '.[] | sort_by(.title)' `
 | jq -r --arg OWNER $OWNER --arg REPONAME $REPONAME '.[] | "- [ ] [\(.title)](https://github.com/\($OWNER)/\($REPONAME)/issues/\(.number))"' `
 | Out-File -Append -FilePath $FILENAME

# FEATURE ISSUE
Add-Content -Path $FILENAME -Value ''
Add-Content -Path $FILENAME -Value '### FEATURE ✨'
gh issue list --json title,number,labels,updatedAt --label 'feature' `
 | jq -s -c '.[] | sort_by(.title)' `
 | jq -r --arg OWNER $OWNER --arg REPONAME $REPONAME '.[] | "- [ ] [\(.title)](https://github.com/\($OWNER)/\($REPONAME)/issues/\(.number))"' `
 | Out-File -Append -FilePath $FILENAME

# STORY ISSUE
Add-Content -Path $FILENAME -Value ''
Add-Content -Path $FILENAME -Value '### STORY 📖'
gh issue list --json title,number,labels,updatedAt --label 'story' `
 | jq -s -c '.[] | sort_by(.title)' `
 | jq -r --arg OWNER $OWNER --arg REPONAME $REPONAME '.[] | "- [ ] [\(.title)](https://github.com/\($OWNER)/\($REPONAME)/issues/\(.number))"' `
 | Out-File -Append -FilePath $FILENAME

# TASK ISSUE
Add-Content -Path $FILENAME -Value ''
Add-Content -Path $FILENAME -Value '### TASK'

Add-Content -Path $FILENAME -Value ''
Add-Content -Path $FILENAME -Value '#### ASSIGNED 👤'
$ASSIGNED_STR = gh issue list --assignee "*" --json number --label 'task'
$ASSIGNED = $ASSIGNED_STR | ConvertFrom-Json
$ASSIGNED.number | ForEach-Object {
    $issuenum = $_
    Search-And-Print-ConnectedIssues -OWNER $OWNER -REPONAME $REPONAME -issuenum $issuenum -FILENAME $FILENAME
}

Add-Content -Path $FILENAME -Value ''
Add-Content -Path $FILENAME -Value '#### NOT ASSIGNED ❓'
$UNASSIGNED_STR = gh issue list --json title,number,labels,assignees,updatedAt --label 'task' --search 'no:assignee' 
$UNASSIGNED = $UNASSIGNED_STR | ConvertFrom-Json
$UNASSIGNED.number | ForEach-Object {
    $issuenum = $_
    Search-And-Print-ConnectedIssues -OWNER $OWNER -REPONAME $REPONAME -issuenum $issuenum -FILENAME $FILENAME
}

# CLOSED ISSUE
Add-Content -Path $FILENAME -Value `n`n
Add-Content -Path $FILENAME -Value '### Closed 🚪'

Add-Content -Path $FILENAME -Value ''
Add-Content -Path $FILENAME -Value '#### ASSIGNED: 👤'
$CLOSEDASSIGNED_STR = gh issue list --assignee "*" --json title,number,labels,closedAt,assignees --state closed --search 'sort:closed-desc' 
$CLOSEDASSIGNED = $CLOSEDASSIGNED_STR | ConvertFrom-Json
$CLOSEDASSIGNED.number | ForEach-Object {
    $issuenum = $_
    Search-And-Print-ConnectedIssues -OWNER $OWNER -REPONAME $REPONAME -issuenum $issuenum -FILENAME $FILENAME
}

Add-Content -Path $FILENAME -Value ''
Add-Content -Path $FILENAME -Value '#### NOT ASSIGNED: ❓'
gh issue list --json title,number,labels,closedAt,assignees --state closed --search 'sort:closed-desc no:assignee' `
 | jq -s -c '.[] | sort_by(.closedAt) | reverse' `
 | jq -r --arg OWNER $OWNER --arg REPONAME $REPONAME '.[] | "- [x] [\(.title)](https://github.com/\($OWNER)/\($REPONAME)/issues/\(.number)) / `@\(.assignees[0].login):\(.assignees[0].name)` / closed:`\(.closedAt)`"' `
 | Out-File -Append -FilePath $FILENAME

$end = Get-Date
Write-Host started at $end.ToString("yyyy-MM-dd HH:mm:ss")
Write-Host total elapsed time: $end.Subtract($start).ToString()
