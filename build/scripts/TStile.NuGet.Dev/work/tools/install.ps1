param($installPath, $toolsPath, $package, $project)

# For each .ts file, gather the matching .js file as a dependent item in the DTE hierarchy
function consolidateTS ($projectItems)
{
	# process this level of items
	foreach($item in $projectItems)
	{
		$filename = $item.Properties.Item("FileName").Value
		#Write-Host ("Properties for ]]" + $filename + "[[")
		
		# for each .ts file
		if($filename.EndsWith(".ts"))
		{
			$jsName = $filename.Substring(0, $filename.Length - 3) + ".js"
			#Write-Host ("target: " + $jsName) 
			foreach($jsItem in $projectItems)
			{
				# for the matching .js file
				if($jsItem.Properties.Item("FileName").Value -eq $jsName)
				{
					$jsFullPath = $jsItem.Properties.Item("FullPath").Value
					#Write-Host ("About to fix: " + $jsFullPath) 
					$jsChildItem = $item.ProjectItems.AddFromFile($jsFullPath)
					$jsChildItem.IsDependentFile = true
					#Write-Host ("IsDependentFile == " + $jsChildItem.IsDependentFile) 
					break
				}
			}
			
			# set build action to 4 == TypeScriptCompile
			#Write-Host ("BuildAction was == " + $item.Properties.Item("BuildAction").Value )
			$item.Properties.Item("BuildAction").Value = 4
			#Write-Host ("BuildAction is == " + $item.Properties.Item("BuildAction").Value )
		}
	}
	
	# recurse on children
	foreach ($anyItem in $projectItems)
	{
		if($anyItem.ProjectItems.Count -gt 0)
		{
			#Write-Host ("push " + $anyItem.Properties.Item("FileName").Value)
			consolidateTS($anyItem.ProjectItems)
			#Write-Host ("pop " + $anyItem.Properties.Item("FileName").Value)
		}
	}
}

# start with items in Scripts\Stile directory
$stileItems = $project.ProjectItems.Item("Scripts").ProjectItems.Item("Stile").ProjectItems

# invoke the recursive function
consolidateTS($stileItems)
